import type * as DTO from '../dtos/events.dto';
import type * as Model from '../models/Digimon';
import type { Player, Party } from '../models/Player';
import type { ImportantItems, ConsumableItems } from '../models/Items';
import type { Journal, Quest, QuestStep } from '../models/Journal';
import { DigievolutionRegistry } from '../logic/DigievolutionRegistry';
import { DigimonExperienceCalculator } from '../logic/DigimonExperienceCalculator';
import { AttributesConverter } from './AttributesConverter';
import { ResistancesConverter } from './ResistancesConverter';
import { EquipmentsConverter } from './EquipmentsConverter';

/**
 * GameConverter
 * Responsible for transforming raw DTOs from the backend into rich Domain Models for the UI.
 */
export class GameConverter {

    public static toPlayerModel(dto: DTO.PlayerDTO | null): Player | null {
        if (!dto) return null;
        return { ...dto };
    }

    public static toDigimonModel(dto: DTO.DigimonDTO | null): Model.Digimon | null {
        if (!dto) return null;

        const basicInfo: Model.BasicInfo = {
            ...dto.basicInfo,
            // Pre-calculate experience fields for the initial model
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
            equippedDigievolutions: dto.equippedDigievolutions.map(evoDto => {
                if (!evoDto) return null;
                return {
                    id: evoDto.id,
                    level: evoDto.level,
                    name: DigievolutionRegistry.getDigievolutionNameById(evoDto.id)
                };
            })
        };
    }

    public static toPartyModel(dto: DTO.PartyDTO | null): Party | null {
        if (!dto) return null;
        return {
            slots: dto.slots.map(slot => this.toDigimonModel(slot))
        };
    }

    public static toImportantItemsModel(dto: DTO.ImportantItemsDTO | null): ImportantItems | null {
        if (!dto) return null;
        return { ...dto };
    }

    public static toConsumableItemsModel(dto: DTO.ConsumableItemsDTO | null): ConsumableItems | null {
        if (!dto) return null;
        return { ...dto };
    }

    /**
     * Note: Full Quest enrichment (descriptions/locations) usually happens 
     * via localization tables. This converter handles the structural mapping.
     */
    public static toJournalModel(dto: DTO.JournalDTO | null): Journal | null {
        if (!dto) return null;
        return {
            mainQuest: this.toQuestModel(dto.mainQuest),
            sideQuests: dto.sideQuests.map(q => this.toQuestModel(q)).filter((q): q is Quest => q !== null)
        };
    }

    public static toDigievolutionName(id: number): string {
        return DigievolutionRegistry.getDigievolutionNameById(id);
    }

    private static toQuestModel(dto: DTO.QuestDTO | null): Quest | null {
        if (!dto || !dto.id) return null;
        return {
            id: dto.id,
            title: dto.title,
            description: dto.description,
            prerequisites: dto.prerequisites.map(p => ({ ...p })),
            steps: dto.steps.map(s => this.toQuestStepModel(s))
        };
    }

    private static toQuestStepModel(dto: DTO.QuestStepDTO): QuestStep {
        return {
            number: dto.number,
            isCompleted: dto.isCompleted,
            prerequisites: dto.prerequisites?.map(p => ({ ...p }))
        };
    }
}
