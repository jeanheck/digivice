import { DigimonStatusCalculator } from '../logic/DigimonStatusCalculator';
import type { DigimonStatus, DigimonStatusType } from '../models/Digimon';

export class DigimonStatusConverter {
    public static convert(
        digimonStatusType: DigimonStatusType,
        fromDigimon: number,
        section: 'attributes' | 'resistances',
        activeEquipaments: any[],
        activeDigievolution: any | null
    ): DigimonStatus {
        const bonusFromEquipaments = DigimonStatusCalculator.calculateBonusFromEquipaments(digimonStatusType, activeEquipaments);
        const bonusFromDigievolution = DigimonStatusCalculator.calculateBonusFromActiveDigievolution(digimonStatusType, section, activeDigievolution);

        return {
            digimonStatusType,
            fromDigimon,
            fromEquipaments: bonusFromEquipaments,
            fromDigievolution: bonusFromDigievolution,
            sumBetweenDigimonAndEquipaments: fromDigimon + bonusFromEquipaments
        };
    }
}
