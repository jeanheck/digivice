import type { Journal, Quest } from "@/models";
import type { JournalDTO } from "@/events/dto/journal.dto";
import type { QuestDTO } from "@/events/dto/journals/quest.dto";
import { QuestSyncer } from "./journals/quest.syncer";

export class JournalSyncer {
  public static sync(previousJournal: Journal, newJournalDto: JournalDTO): void {
    if (newJournalDto.mainQuest !== undefined) {
      QuestSyncer.sync(previousJournal.mainQuest, newJournalDto.mainQuest);
    }

    this.syncQuests(previousJournal.sideQuests, newJournalDto.sideQuests);
    this.syncQuests(previousJournal.legendaryWeapons, newJournalDto.legendaryWeapons);
    this.syncQuests(previousJournal.driAgents, newJournalDto.driAgents);
    this.syncQuests(previousJournal.duelIsland, newJournalDto.duelIsland);
  }

  private static syncQuests(previousQuests: Quest[], newQuestDtos: QuestDTO[] | undefined): void {
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
