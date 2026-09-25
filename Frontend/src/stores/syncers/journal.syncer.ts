import type { Journal, Quest } from "@/models";
import type * as Events from "@/events/events.map";
import { QuestSyncer } from "./journals/quest.syncer";

export class JournalSyncer {
  public static sync(previousJournal: Journal, newJournalDto: Events.JournalDTO): void {
    if (newJournalDto.mainQuest !== undefined) {
      QuestSyncer.sync(previousJournal.mainQuest, newJournalDto.mainQuest);
    }

    this.syncQuests(previousJournal.sideQuests, newJournalDto.sideQuests);
    this.syncQuests(previousJournal.legendaryWeapons, newJournalDto.legendaryWeapons);
    this.syncQuests(previousJournal.driAgents, newJournalDto.driAgents);
    this.syncQuests(previousJournal.duelIsland, newJournalDto.duelIsland);
  }

  private static syncQuests(previousQuests: Quest[], newQuestDtos: Events.QuestDTO[] | undefined): void {
    if (!newQuestDtos) {
      return;
    }

    newQuestDtos.forEach((newQuestDto) => {
      const previousQuest = previousQuests.find((quest) => quest.id === newQuestDto.id);
      if (previousQuest) {
        QuestSyncer.sync(previousQuest, newQuestDto);
      }
    });
  }
}
