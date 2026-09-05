import type { Journal, Party } from "@/models";
import { QuestModalPresenter } from "@/presenters/journal/quest-modal.presenter";
import { QuestRepository } from "@/repositories/quest.repository";
import type { JournalViewModel } from "@/viewmodels/quest/journal.viewmodel";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";

export class JournalPresenter {
  public static getJournalViewModel(journal: Journal, party: Party): JournalViewModel {
    const mainQuestRaw = QuestRepository.getMainQuestRaw();
    const mainQuestViewModel =
      journal.mainQuest === null
        ? null
        : QuestModalPresenter.getQuestViewModel(journal, mainQuestRaw.id, party);

    const sideQuestsViewModels = QuestRepository.getSideQuestsRaw()
      .map((sideQuestRaw) =>
        QuestModalPresenter.getQuestViewModel(journal, sideQuestRaw.id, party),
      )
      .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

    const legendaryWeaponsViewModels = QuestRepository.getLegendaryWeaponsRaw()
      .map((legendaryWeaponRaw) =>
        QuestModalPresenter.getQuestViewModel(journal, legendaryWeaponRaw.id, party),
      )
      .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

    const driAgentsViewModels = QuestRepository.getDriAgentsRaw()
      .map((driAgentRaw) =>
        QuestModalPresenter.getQuestViewModel(journal, driAgentRaw.id, party),
      )
      .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

    const duelIslandViewModels = QuestRepository.getDuelIslandRaw()
      .map((duelIslandQuestRaw) =>
        QuestModalPresenter.getQuestViewModel(journal, duelIslandQuestRaw.id, party),
      )
      .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

    return {
      mainQuest: mainQuestViewModel,
      sideQuests: sideQuestsViewModels,
      legendaryWeapons: legendaryWeaponsViewModels,
      driAgents: driAgentsViewModels,
      duelIsland: duelIslandViewModels,
    };
  }
}
