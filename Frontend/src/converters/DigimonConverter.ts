import type * as DTO from '../dtos/events.dto';
import type * as Model from '../models/Digimon';
import { DigimonExperienceCalculator } from '../logic/DigimonExperienceCalculator';
import { AttributesConverter } from './AttributesConverter';
import { ResistancesConverter } from './ResistancesConverter';
import { EquipmentsConverter } from './EquipmentsConverter';
import { DigievolutionsConverter } from './DigievolutionsConverter';

export class DigimonConverter {
    public static convert(digimon: DTO.DigimonDTO | null): Model.Digimon | null {
        if (!digimon) return null;

        const basicInfo: Model.BasicInfo = {
            ...digimon.basicInfo,
            experienceToReachNextLevel: DigimonExperienceCalculator.getRequiredExpForNextLevel(
                digimon.basicInfo.name,
                digimon.basicInfo.level
            ),
            experiencePercentageToReachNextLevel: DigimonExperienceCalculator.getProgressPercentageForNextLevel(
                digimon.basicInfo.name,
                digimon.basicInfo.level,
                digimon.basicInfo.experience
            )
        };

        const equipments = EquipmentsConverter.convert(digimon.equipments);
        const activeDigievolutionId = digimon.activeDigievolutionId;

        return {
            slotIndex: digimon.slotIndex,
            basicInfo,
            attributes: AttributesConverter.convert(digimon.attributes, equipments, activeDigievolutionId),
            resistances: ResistancesConverter.convert(digimon.resistances, equipments, activeDigievolutionId),
            equipments,
            activeDigievolutionId,
            digievolutions: DigievolutionsConverter.convert(digimon.digievolutions)
        };
    }
}
