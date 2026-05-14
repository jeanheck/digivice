import type { State } from '../models/State';
import type { Journal } from '../models/Journal';

export class JournalUpdater {
    public static update(state: State, journal: Journal | null) {
        state.journal = journal;
    }
}
