import type { State } from '../models/State';
import type { ImportantItems } from '../models/Items';

export class ImportantItemsUpdater {
    public static update(state: State, importantItems: ImportantItems | null) {
        state.importantItems = importantItems;
    }
}
