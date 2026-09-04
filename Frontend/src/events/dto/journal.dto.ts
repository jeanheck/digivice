import type { QuestDTO } from "./journals/quest.dto";
import type { NpcDTO } from "./npcs/npc.dto";

export interface JournalDTO {
  mainQuest?: QuestDTO | null;
  sideQuests?: QuestDTO[];
  legendaryWeapons?: QuestDTO[];
  driAgents?: QuestDTO[];
  duelIsland?: QuestDTO[];
  npcs?: NpcDTO[];
}
