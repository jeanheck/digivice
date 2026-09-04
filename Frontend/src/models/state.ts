import type { ImportantItems } from "./important-items";
import type { Party } from "./party/party";
import type { DigimonBattle } from "./digimon-battle";
import type { CardBattle } from "./card-battle";
import type { Auctions } from "./auctions";
import type { Journal } from "./journal/journal";
import type { Player } from "./player";

export interface State {
  player: Player | null;
  importantItems: ImportantItems | null;
  party: Party | null;
  digimonBattle: DigimonBattle | null;
  cardBattle: CardBattle | null;
  auctions: Auctions | null;
  journal: Journal | null;
}
