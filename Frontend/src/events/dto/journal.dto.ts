import type { QuestDTO } from './journals/quest.dto';

export interface JournalDTO {
    mainQuest?: QuestDTO | null;
    sideQuests?: QuestDTO[];
    legendaryWeapons?: QuestDTO[];
    driAgents?: QuestDTO[];
}
