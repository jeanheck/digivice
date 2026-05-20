import type { PartyDTO } from '../dto/party.dto';
import type { Party, DigimonSlot } from '../../models/Party';
import { DigimonSlotConverter } from './digimon-slot.converter';
import { PartyCalculator } from '../../logic/PartyCalculator';

export class PartyConverter {
    public static convert(party: PartyDTO | null): Party | null {
        if (!party || !party.slots) return null;

        const slots: DigimonSlot[] = [];
        for (let i = 0; i < 3; i++) {
            slots.push({
                index: i,
                digimonId: null,
                digimon: null
            });
        }

        party.slots.forEach(slotDto => {
            if (slotDto && slotDto.index >= 0 && slotDto.index < slots.length) {
                const converted = DigimonSlotConverter.convert(slotDto);
                if (converted) {
                    slots[slotDto.index] = converted;
                }
            }
        });

        return {
            slots,
            groupCharisma: PartyCalculator.calculateGroupCharisma(slots)
        };
    }
}
