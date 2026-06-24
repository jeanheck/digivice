import type { QuestViewModel } from "./quest.viewmodel";

export interface JournalViewModel {
    mainQuest: QuestViewModel | null;
    sideQuests: QuestViewModel[];
    legendaryWeapons: QuestViewModel[];
    driAgents: QuestViewModel[];
}
