import type { Player } from './Player';
import type { Party } from './Party';
import type { Journal } from './Journal';

export interface State {
    player: Player | null;
    party: Party | null;
    journal: Journal | null;
}
