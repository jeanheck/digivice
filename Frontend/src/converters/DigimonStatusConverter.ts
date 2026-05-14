import { DigimonStatusCalculator } from '../logic/DigimonStatusCalculator';
import type { DigimonStatus } from '../models/Digimon';

export class DigimonStatusConverter {
    public static convert(
        type: string,
        fromDigimon: number,
        section: 'attributes' | 'resistances',
        filteredEquipments: any[],
        activeDigievolution: any | null
    ): DigimonStatus {
        const fromEquipaments = DigimonStatusCalculator.getEquipBonus(type, filteredEquipments);
        const fromDigievolution = DigimonStatusCalculator.getDigiBonus(type, section, activeDigievolution);

        return {
            type,
            fromDigimon,
            fromEquipaments,
            fromDigievolution,
            sumBetweenDigimonAndEquipaments: fromDigimon + fromEquipaments
        };
    }
}
