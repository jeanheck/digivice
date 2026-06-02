import type { Journal, Quest } from "@/models";
import { QuestConverter } from "@/presenters/converter/quest.converter";
import { QuestRepository } from "@/repositories/quest.repository";
import type { JournalViewModel } from "@/viewmodels/quest/journal.viewmodel";

export class JournalPresenter {
    public static getJournalViewModel(journal: Journal): JournalViewModel {
        const mainQuestRaw = QuestRepository.getMainQuestRaw();
        const mainQuestViewModel = journal.mainQuest === null
            ? null
            : QuestConverter.convert(mainQuestRaw, journal.mainQuest, { calculateNewStatus: false });

        const sideQuestsRaw = QuestRepository.getSideQuestsRaw();
        const sideQuestsViewModels = sideQuestsRaw.map((sideQuestRaw) => {
            return QuestConverter.convert(
                sideQuestRaw,
                JournalPresenter.getSideQuestFromJournal(journal, sideQuestRaw.id),
                { calculateNewStatus: true }
            );
        });

        return {
            mainQuest: mainQuestViewModel,
            sideQuests: sideQuestsViewModels,
        };
    }

    private static getSideQuestFromJournal(journal: Journal, sideQuestId: string): Quest {
        return journal.sideQuests.find((quest) => quest.id === sideQuestId)!;
    }
}
