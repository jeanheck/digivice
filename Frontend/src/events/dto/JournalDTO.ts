import type { QuestDTO } from './Journals/QuestDTO';

export interface JournalDTO {
    mainQuest?: QuestDTO | null;
    sideQuests?: QuestDTO[];
}
