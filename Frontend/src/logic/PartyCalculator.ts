import { MathUtils } from '@/utils/MathUtils';
import type { Digimon } from '../models';

export class PartyCalculator {
    public static calculateGroupCharisma(slots: (Digimon | null)[]): number {
        const allDigimonsCharisma = slots.map(digimon => {
            if (!digimon) {
                return 0;
            }
            return digimon.attributes.charisma.sumBetweenDigimonAndEquipaments;
        });

        return MathUtils.Sum(allDigimonsCharisma);
    }
}
