import type { JournalDTO } from '../dto/journal.dto';
import type { QuestDTO } from '../dto/journals/quest.dto';
import type { Journal } from '../../models';
import { QuestConverter } from './journals/quest.converter';

export class JournalConverter {
    public static convert(journalDto: Required<JournalDTO>): Journal {
        return {
            mainQuest: journalDto.mainQuest ? QuestConverter.convert(journalDto.mainQuest as Required<QuestDTO>) : null,
            sideQuests: journalDto.sideQuests
                ? journalDto.sideQuests.map(q => QuestConverter.convert(q as Required<QuestDTO>))
                : [],
            legendaryWeapons: journalDto.legendaryWeapons
                ? journalDto.legendaryWeapons.map((questDto) => QuestConverter.convert(questDto as Required<QuestDTO>))
                : [],
        };
    }
}
