import type { Player } from "./player";
import type { Party } from "./party/party";
import type { Battle } from "./battle";
import type { Journal } from "./journal/journal";

export interface State {
  player: Player | null;
  party: Party | null;
  battle: Battle | null;
  journal: Journal | null;
}
