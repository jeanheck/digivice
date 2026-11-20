import type { QuestResource } from "@/memory/resources/journals/quest.resource";

export interface JournalResource {
  mainQuest: QuestResource;
  sideQuests: QuestResource[];
  legendaryWeapons: QuestResource[];
  driAgents: QuestResource[];
  duelIsland: QuestResource[];
}
