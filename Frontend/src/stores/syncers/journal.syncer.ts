import type { Journal } from '../../models/Journal';
import type * as Events from '../../events/events.map';
import { QuestConverter } from '../../events/converters/quest.converter';

export class JournalSyncer {
    public static sync(journal: Journal, journalDto: Events.JournalDTO): void {
        if (journalDto.mainQuest !== undefined) {
            journal.mainQuest = journalDto.mainQuest
                ? QuestConverter.convert(journalDto.mainQuest as Required<Events.QuestDTO>)
                : null;
        }

        if (journalDto.sideQuests !== undefined) {
            journalDto.sideQuests.forEach((questDto) => {
                if (!questDto) {
                    return;
                }

                const converted = QuestConverter.convert(questDto as Required<Events.QuestDTO>);
                const existingIndex = journal.sideQuests.findIndex((q) => q.id === converted.id);

                if (existingIndex !== -1) {
                    journal.sideQuests[existingIndex] = converted;
                } else {
                    journal.sideQuests.push(converted);
                }
            });
        }
    }
}
