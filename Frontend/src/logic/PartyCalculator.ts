import { MathUtils } from '@/utils/MathUtils';
import type { DigimonSlot } from '../models';

export class PartyCalculator {
    public static calculateGroupCharisma(slots: DigimonSlot[]): number {
        const allDigimonsCharisma = slots.map(slot => {
            if (!slot || !slot.digimon) {
                return 0;
            }
            return slot.digimon.attributes.charisma.sumBetweenDigimonAndEquipaments;
        });

        return MathUtils.Sum(allDigimonsCharisma);
    }
}
