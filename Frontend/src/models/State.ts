import type { Player, Party } from './Player';
import type { ImportantItems } from './Items';
import type { Journal } from './Journal';
import type { AreaInformation } from './AreaInformation';

export interface State {
    player: Player | null;
    party: Party | null;
    importantItems: ImportantItems | null;
    journal: Journal | null;
    areaInformation: AreaInformation | null;
}
