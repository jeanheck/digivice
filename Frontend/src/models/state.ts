import type { Player } from "./player";
import type { ImportantItems } from "./important-items";
import type { Party } from "./party/party";
import type { DigimonBattle } from "./digimon-battle";
import type { CardBattle } from "./card-battle";
import type { Journal } from "./journal/journal";

export interface State {
  player: Player | null;
  importantItems: ImportantItems | null;
  party: Party | null;
  digimonBattle: DigimonBattle | null;
  cardBattle: CardBattle | null;
  journal: Journal | null;
}
