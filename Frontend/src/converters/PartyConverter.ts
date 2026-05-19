import type * as DTO from '../dtos/events.dto';
import type { Party } from '../models/Player';
import { DigimonConverter } from './DigimonConverter';
import { PartyCalculator } from '../logic/PartyCalculator';
import type { Digimon } from '../models/Digimon';

export class PartyConverter {
    public static convert(party: DTO.PartyDTO | null): Party | null {
        if (!party || !party.slots) return null;
        
        const slots: (Digimon | null)[] = [null, null, null];
        
        party.slots.forEach(slotDto => {
            if (slotDto && slotDto.index >= 0 && slotDto.index < slots.length) {
                slots[slotDto.index] = slotDto.digimon 
                    ? DigimonConverter.convert(slotDto.digimon, slotDto.index) 
                    : null;
            }
        });
        
        return {
            slots,
            groupCharisma: PartyCalculator.calculateGroupCharisma(slots)
        };
    }
}
