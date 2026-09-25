import type { PlayerDTO } from "./player.dto";
import type { ImportantItemsDTO } from "./important-items.dto";
import type { PartyDTO } from "./party.dto";
import type { DigimonBattleDTO } from "./digimon-battle.dto";
import type { CardBattleDTO } from "./card-battle.dto";
import type { AuctionsDTO } from "./auctions.dto";
import type { NpcsDTO } from "./npcs.dto";
import type { JournalDTO } from "./journal.dto";

export interface StateDTO {
  player: Required<PlayerDTO> | null;
  importantItems: Required<ImportantItemsDTO> | null;
  party: Required<PartyDTO> | null;
  digimonBattle: Required<DigimonBattleDTO> | null;
  cardBattle: Required<CardBattleDTO> | null;
  auctions: Required<AuctionsDTO> | null;
  npcs: Required<NpcsDTO> | null;
  journal: Required<JournalDTO> | null;
}
