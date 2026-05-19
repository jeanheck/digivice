import type * as DTO from '../dtos/events.dto';
import type { Journal, Quest, QuestStep, Requisite } from '../models/Journal';

export class JournalConverter {
    public static convert(journal: DTO.JournalDTO | null): Journal | null {
        if (!journal) return null;
        return {
            mainQuest: this.toQuestModel(journal.mainQuest ?? null),
            sideQuests: journal.sideQuests 
                ? journal.sideQuests.map(q => this.toQuestModel(q)).filter((q): q is Quest => q !== null) 
                : []
        };
    }

    private static toQuestModel(dto: DTO.QuestDTO | null): Quest | null {
        if (!dto || !dto.id) return null;
        return {
            id: dto.id,
            title: '', // Preenchido via getLocalizedQuest no composable de localização
            description: '',
            prerequisites: dto.requisites ? dto.requisites.map(r => this.toRequisiteModel(r)) : [],
            steps: dto.steps ? dto.steps.map(s => this.toQuestStepModel(s)) : []
        };
    }

    private static toQuestStepModel(dto: DTO.StepDTO): QuestStep {
        return {
            number: dto.number,
            isCompleted: dto.value !== undefined && dto.value !== 0,
            prerequisites: dto.requisites ? dto.requisites.map(r => this.toRequisiteModel(r)) : []
        };
    }

    private static toRequisiteModel(dto: DTO.RequisiteDTO): Requisite & { itemKey?: string; id?: string } {
        return {
            id: dto.id,
            itemKey: dto.id, // Mapeia id para itemKey permitindo tradução local automática
            description: '', // Preenchido pelo localizador
            isDone: dto.value !== undefined && dto.value !== 0
        };
    }
}
