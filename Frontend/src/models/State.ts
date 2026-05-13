import type { Player, Party } from './Player';
import type { ImportantItems, ConsumableItems } from './Items';
import type { Journal } from './Journal';

export interface State {
    player: Player | null;
    party: Party | null;
    importantItems: ImportantItems | null;
    consumableItems: ConsumableItems | null;
    journal: Journal | null;
}
