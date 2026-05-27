import type { QuestViewModel } from "./quest.viewmodel";

export interface JournalViewModel {
    mainQuest: QuestViewModel;
    sideQuests: QuestViewModel[];
}
