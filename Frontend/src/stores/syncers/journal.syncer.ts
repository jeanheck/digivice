import type { Journal } from '../../models/Journal';
import type * as Events from '../../events/events.map';
import { QuestSyncer } from './quest.syncer';

export class JournalSyncer {
    public static sync(previousJournal: Journal, newJournalDto: Events.JournalDTO): void {
        if (newJournalDto.mainQuest && previousJournal.mainQuest) {
            QuestSyncer.sync(previousJournal.mainQuest, newJournalDto.mainQuest);
        }

        if (newJournalDto.sideQuests && newJournalDto.sideQuests.length > 0) {
            newJournalDto.sideQuests.forEach((newSideQuestDto) => {
                const previousSideQuest = previousJournal.sideQuests.find((q) => q.id === newSideQuestDto.id);

                if (previousSideQuest) {
                    QuestSyncer.sync(previousSideQuest, newSideQuestDto);
                }
            });
        }
    }
}

