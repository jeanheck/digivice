import type * as DTO from '../events/dto/events.dto';
import type * as Model from '../models/Digimon';
import { DigimonExperienceCalculator } from '../logic/DigimonExperienceCalculator';
import { AttributesConverter } from './AttributesConverter';
import { ResistancesConverter } from './ResistancesConverter';
import { EquipmentsConverter } from './EquipmentsConverter';
import { DigievolutionsConverter } from './DigievolutionsConverter';
import { DigievolutionRegistry } from '../logic/DigievolutionRegistry';

export class DigimonConverter {
    public static convert(digimon: DTO.DigimonDTO | null, slotIndex: number = 0): Model.Digimon | null {
        if (!digimon) return null;

        // Resolve o nome do Digimon a partir da evolução ativa
        const name = digimon.activeDigievolutionId !== undefined && digimon.activeDigievolutionId !== null
            ? DigievolutionRegistry.getDigievolutionNameById(digimon.activeDigievolutionId)
            : 'Unknown';

        const level = digimon.level ?? 1;
        const experience = digimon.experience ?? 0;
        
        const currentHP = digimon.vitals?.currentHP ?? 0;
        const maxHP = digimon.vitals?.maxHP ?? 0;
        const currentMP = digimon.vitals?.currentMP ?? 0;
        const maxMP = digimon.vitals?.maxMP ?? 0;

        const basicInfo: Model.BasicInfo = {
            name,
            level,
            experience,
            currentHP,
            maxHP,
            currentMP,
            maxMP,
            experienceToReachNextLevel: DigimonExperienceCalculator.getRequiredExpForNextLevel(name, level),
            experiencePercentageToReachNextLevel: DigimonExperienceCalculator.getProgressPercentageForNextLevel(name, level, experience)
        };

        const equipments = EquipmentsConverter.convert(digimon.equipments ?? null);
        const activeDigievolutionId = digimon.activeDigievolutionId ?? null;

        return {
            slotIndex,
            basicInfo,
            attributes: AttributesConverter.convert(digimon.attributes ?? null, equipments, activeDigievolutionId),
            resistances: ResistancesConverter.convert(digimon.resistances ?? null, equipments, activeDigievolutionId),
            equipments,
            activeDigievolutionId,
            digievolutions: DigievolutionsConverter.convert(digimon.digievolutions ?? null)
        };
    }
}
