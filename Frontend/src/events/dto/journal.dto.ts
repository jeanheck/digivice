import type { QuestDTO } from "./journals/quest.dto";

export interface JournalDTO {
  mainQuest?: QuestDTO;
  sideQuests?: QuestDTO[];
  legendaryWeapons?: QuestDTO[];
  driAgents?: QuestDTO[];
  duelIsland?: QuestDTO[];
}
