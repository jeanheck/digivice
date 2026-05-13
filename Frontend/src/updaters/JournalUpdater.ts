import type { State } from '../models/State';
import type * as Events from '../dtos/events.dto';
import { GameConverter } from '../converters/GameConverter';

export class JournalUpdater {
    public static updateJournal(state: State | null, event: Events.JournalChangedDTO) {
        if (state) {
            state.journal = GameConverter.toJournalModel(event.journal);
        }
    }
}
