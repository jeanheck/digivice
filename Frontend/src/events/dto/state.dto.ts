import type { DeepRequired } from "./deep-required";
import type { PlayerDTO } from "./player.dto";
import type { ImportantItemsDTO } from "./important-items.dto";
import type { PartyDTO } from "./party.dto";
import type { DigimonBattleDTO } from "./digimon-battle.dto";
import type { CardBattleDTO } from "./card-battle.dto";
import type { AuctionsDTO } from "./auctions.dto";
import type { NpcsDTO } from "./npcs.dto";
import type { JournalDTO } from "./journal.dto";

export interface StateDTO {
  player: DeepRequired<PlayerDTO> | null;
  importantItems: DeepRequired<ImportantItemsDTO> | null;
  party: DeepRequired<PartyDTO> | null;
  digimonBattle: DeepRequired<DigimonBattleDTO> | null;
  cardBattle: DeepRequired<CardBattleDTO> | null;
  auctions: DeepRequired<AuctionsDTO> | null;
  npcs: DeepRequired<NpcsDTO> | null;
  journal: DeepRequired<JournalDTO> | null;
}
