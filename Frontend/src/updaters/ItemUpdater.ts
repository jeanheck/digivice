import type { State } from '../models/State';
import type * as Events from '../dtos/events.dto';
import { GameConverter } from '../converters/GameConverter';

export class ItemUpdater {
    public static updateImportantItems(state: State | null, event: Events.ImportantItemsChangedDTO) {
        if (state) {
            state.importantItems = GameConverter.toImportantItemsModel(event.importantItems);
        }
    }
}
