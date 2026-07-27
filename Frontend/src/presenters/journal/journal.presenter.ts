import type { DigimonSlot, Journal } from "@/models";
import { FooterPresenter } from "@/presenters/footer/footer.presenter";
import { QuestModalPresenter } from "@/presenters/journal/quest-modal.presenter";
import { QuestRepository } from "@/repositories/quest.repository";
import type { JournalViewModel } from "@/viewmodels/quest/journal.viewmodel";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";

export class JournalPresenter {
  public static getJournalViewModel(
    journal: Journal,
    digimonSlots: DigimonSlot[],
  ): JournalViewModel {
    const partyLevel = FooterPresenter.getPartyLevel(digimonSlots);
    const mainQuestRaw = QuestRepository.getMainQuestRaw();
    const mainQuestViewModel =
      journal.mainQuest === null
        ? null
        : QuestModalPresenter.getQuestViewModel(journal, mainQuestRaw.id, partyLevel);

    const sideQuestsViewModels = QuestRepository.getSideQuestsRaw()
      .map((sideQuestRaw) =>
        QuestModalPresenter.getQuestViewModel(journal, sideQuestRaw.id, partyLevel),
      )
      .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

    const legendaryWeaponsViewModels = QuestRepository.getLegendaryWeaponsRaw()
      .map((legendaryWeaponRaw) =>
        QuestModalPresenter.getQuestViewModel(journal, legendaryWeaponRaw.id, partyLevel),
      )
      .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

    const driAgentsViewModels = QuestRepository.getDriAgentsRaw()
      .map((driAgentRaw) =>
        QuestModalPresenter.getQuestViewModel(journal, driAgentRaw.id, partyLevel),
      )
      .filter((questViewModel): questViewModel is QuestViewModel => questViewModel !== null);

    return {
      mainQuest: mainQuestViewModel,
      sideQuests: sideQuestsViewModels,
      legendaryWeapons: legendaryWeaponsViewModels,
      driAgents: driAgentsViewModels,
    };
  }
}
