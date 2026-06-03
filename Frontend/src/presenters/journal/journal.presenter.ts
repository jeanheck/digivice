import type { Journal } from "@/models";
import { QuestModalPresenter } from "@/presenters/journal/quest-modal.presenter";
import { QuestRepository } from "@/repositories/quest.repository";
import type { JournalViewModel } from "@/viewmodels/quest/journal.viewmodel";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";

export class JournalPresenter {
    public static getJournalViewModel(journal: Journal): JournalViewModel {
        const mainQuestRaw = QuestRepository.getMainQuestRaw();
        const mainQuestViewModel = journal.mainQuest === null
            ? null
            : QuestModalPresenter.getQuestViewModel(journal, mainQuestRaw.id);

        const sideQuestsViewModels = QuestRepository.getSideQuestsRaw()
            .map((sideQuestRaw) => QuestModalPresenter.getQuestViewModel(journal, sideQuestRaw.id))
            .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

        return {
            mainQuest: mainQuestViewModel,
            sideQuests: sideQuestsViewModels,
        };
    }
}
