import type { QuestAddresses } from "@/memory/addresses/journals/quest.addresses";

export interface JournalAddresses {
  mainQuest: QuestAddresses;
  sideQuests: QuestAddresses[];
  legendaryWeapons: QuestAddresses[];
  driAgents: QuestAddresses[];
  duelIsland: QuestAddresses[];
}
