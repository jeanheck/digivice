import type { PlayerDTO } from "./player.dto";
import type { ImportantItemsDTO } from "./important-items.dto";
import type { PartyDTO } from "./party.dto";
import type { BattleDTO } from "./battle.dto";
import type { CardBattleDTO } from "./card-battle.dto";
import type { JournalDTO } from "./journal.dto";

export interface StateDTO {
  player: Required<PlayerDTO> | null;
  importantItems: Required<ImportantItemsDTO> | null;
  party: Required<PartyDTO> | null;
  battle: Required<BattleDTO> | null;
  cardBattle: Required<CardBattleDTO> | null;
  journal: Required<JournalDTO> | null;
}
