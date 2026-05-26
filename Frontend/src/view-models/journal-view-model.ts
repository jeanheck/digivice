import type { QuestViewModel } from "./quest-view-model";

export interface JournalViewModel {
    mainQuest: QuestViewModel;
    sideQuests: QuestViewModel[];
}
