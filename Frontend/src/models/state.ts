import type { Player } from "./player";
import type { ImportantItems } from "./important-items";
import type { Party } from "./party/party";
import type { Battle } from "./battle";
import type { Journal } from "./journal/journal";

export interface State {
  player: Player | null;
  importantItems: ImportantItems | null;
  party: Party | null;
  battle: Battle | null;
  journal: Journal | null;
}
