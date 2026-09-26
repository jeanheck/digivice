import type { HealthDTO } from "./dto/health.dto";
import type { StateDTO } from "./dto/state.dto";
import type { PlayerDTO } from "./dto/player.dto";
import type { ImportantItemsDTO } from "./dto/important-items.dto";
import type { PartyDTO } from "./dto/party.dto";
import type { DigimonBattleDTO } from "./dto/digimon-battle.dto";
import type { CardBattleDTO } from "./dto/card-battle.dto";
import type { AuctionsDTO } from "./dto/auctions.dto";
import type { NpcsDTO } from "./dto/npcs.dto";
import type { JournalDTO } from "./dto/journal.dto";

// Strict mapping of SignalR and client events
export interface EventsMap {
  HealthChanged: HealthDTO;
  InitialState: StateDTO;
  PlayerChanged: PlayerDTO;
  ImportantItemsChanged: ImportantItemsDTO;
  PartyChanged: PartyDTO;
  DigimonBattleChanged: DigimonBattleDTO;
  CardBattleChanged: CardBattleDTO;
  AuctionsChanged: AuctionsDTO;
  NpcsChanged: NpcsDTO;
  JournalChanged: JournalDTO;
  HubConnectionStatusChanged: {
    isConnected: boolean;
    errorMessage?: string;
    preserveGameState?: boolean;
  };
}
