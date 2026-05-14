import type * as DTO from '../dtos/events.dto';
import type * as Model from '../models/Digimon';
import { DigimonExperienceCalculator } from '../logic/DigimonExperienceCalculator';
import { AttributesConverter } from './AttributesConverter';
import { ResistancesConverter } from './ResistancesConverter';
import { EquipmentsConverter } from './EquipmentsConverter';
import { EquippedDigievolutionsConverter } from './EquippedDigievolutionsConverter';

export class DigimonConverter {
    public static convert(dto: DTO.DigimonDTO | null): Model.Digimon | null {
        if (!dto) return null;

        const basicInfo: Model.BasicInfo = {
            ...dto.basicInfo,
            experienceToReachNextLevel: DigimonExperienceCalculator.getRequiredExpForNextLevel(
                dto.basicInfo.name,
                dto.basicInfo.level
            ),
            experiencePercentageToReachNextLevel: DigimonExperienceCalculator.getProgressPercentageForNextLevel(
                dto.basicInfo.name,
                dto.basicInfo.level,
                dto.basicInfo.experience
            )
        };

        const equipments = EquipmentsConverter.convert(dto.equipments);
        const activeDigievolutionId = dto.activeDigievolutionId;

        return {
            slotIndex: dto.slotIndex,
            basicInfo,
            attributes: AttributesConverter.convert(dto.attributes, equipments, activeDigievolutionId),
            resistances: ResistancesConverter.convert(dto.resistances, equipments, activeDigievolutionId),
            equipments,
            activeDigievolutionId,
            equippedDigievolutions: EquippedDigievolutionsConverter.convert(dto.equippedDigievolutions)
        };
    }
}
