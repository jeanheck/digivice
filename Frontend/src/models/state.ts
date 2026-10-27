import type { ImportantItems } from "./important-items";
import type { Party } from "./party/party";
import type { DigimonBattle } from "./digimon-battle";
import type { CardBattle } from "./card-battle";
import type { Auctions } from "./auctions";
import type { Npcs } from "./npcs";
import type { Journal } from "./journal/journal";
import type { Player } from "./player";
import type { CardInventory } from "./card-inventory";

export interface State {
  player: Player;
  importantItems: ImportantItems;
  party: Party;
  digimonBattle: DigimonBattle;
  cardBattle: CardBattle;
  auctions: Auctions;
  npcs: Npcs;
  journal: Journal;
  cardInventory: CardInventory;
}
