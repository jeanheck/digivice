import type { PartyDTO } from '../events/dto/PartyDTO';
import type { Party } from '../models/Player';
import { DigimonSlotConverter } from './DigimonSlotConverter';
import { PartyCalculator } from '../logic/PartyCalculator';
import type { Digimon } from '../models/Digimon';

export class PartyConverter {
    public static convert(party: PartyDTO | null): Party | null {
        if (!party || !party.slots) return null;
        
        const slots: (Digimon | null)[] = [null, null, null];
        
        party.slots.forEach(slotDto => {
            if (slotDto && slotDto.index >= 0 && slotDto.index < slots.length) {
                slots[slotDto.index] = DigimonSlotConverter.convert(slotDto);
            }
        });
        
        return {
            slots,
            groupCharisma: PartyCalculator.calculateGroupCharisma(slots)
        };
    }
}
