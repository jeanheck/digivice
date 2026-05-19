import type { JournalDTO } from '../events/dto/JournalDTO';
import type { Journal, Quest } from '../models/Journal';
import { QuestConverter } from './QuestConverter';

export class JournalConverter {
    public static convert(journal: JournalDTO | null): Journal | null {
        if (!journal) return null;
        return {
            mainQuest: QuestConverter.convert(journal.mainQuest ?? null),
            sideQuests: journal.sideQuests 
                ? journal.sideQuests.map(q => QuestConverter.convert(q)).filter((q): q is Quest => q !== null) 
                : []
        };
    }
}
