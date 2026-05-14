import type * as DTO from '../dtos/events.dto';
import type { Journal, Quest, QuestStep } from '../models/Journal';

export class JournalConverter {
    /**
     * Note: Full Quest enrichment (descriptions/locations) usually happens 
     * via localization tables. This converter handles the structural mapping.
     */
    public static convert(journal: DTO.JournalDTO | null): Journal | null {
        if (!journal) return null;
        return {
            mainQuest: this.toQuestModel(journal.mainQuest),
            sideQuests: journal.sideQuests.map(q => this.toQuestModel(q)).filter((q): q is Quest => q !== null)
        };
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
