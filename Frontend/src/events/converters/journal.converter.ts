import type { JournalDTO } from '../dto/journal.dto';
import type { QuestDTO } from '../dto/journals/quest.dto';
import type { Journal } from '../../models/Journal';
import { QuestConverter } from './quest.converter';

export class JournalConverter {
    public static convert(journal: Required<JournalDTO>): Journal {
        return {
            mainQuest: journal.mainQuest ? QuestConverter.convert(journal.mainQuest as Required<QuestDTO>) : null,
            sideQuests: journal.sideQuests
                ? journal.sideQuests.map(q => QuestConverter.convert(q as Required<QuestDTO>))
                : []
        };
    }
}
