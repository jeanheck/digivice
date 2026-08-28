// Export everything from individual modular files in dto folder
export type { PlayerDTO } from "./dto/player.dto";
export type { ImportantItemsDTO } from "./dto/important-items.dto";
export type { ConnectionDTO, EmulatorConnectionStatusChangedDTO } from "./dto/connection.dto";
export type { PartyDTO } from "./dto/party.dto";
export type { BattleDTO } from "./dto/battle.dto";
export type { CardBattleDTO } from "./dto/card-battle.dto";
export type { EnemyDTO } from "./dto/battles/enemy.dto";
export type { JournalDTO } from "./dto/journal.dto";
export type { StateDTO } from "./dto/state.dto";

export type { AuctionDTO } from "./dto/auctions/auction.dto";
export type { NpcDTO } from "./dto/npcs/npc.dto";
export type { NpcBattleDTO } from "./dto/npcs/npc-battle.dto";
export type { QuestDTO } from "./dto/journals/quest.dto";
export type { RequisiteDTO } from "./dto/journals/quests/requisite.dto";
export type { StepDTO } from "./dto/journals/quests/step.dto";

export type { DigimonDTO } from "./dto/parties/digimon.dto";
export type { DigimonSlotDTO } from "./dto/parties/digimon-slot.dto";
export type { VitalDTO } from "./dto/parties/digimons/vital.dto";
export type { AttributesDTO } from "./dto/parties/digimons/attributes.dto";
export type { ResistancesDTO } from "./dto/parties/digimons/resistances.dto";
export type { EquipmentsDTO } from "./dto/parties/digimons/equipments.dto";
export type { DigievolutionDTO } from "./dto/parties/digimons/digievolution.dto";
export type { DigievolutionSlotDTO } from "./dto/parties/digimons/digievolution-slot.dto";
export type { StoredDigievolutionDTO } from "./dto/parties/digimons/stored-digievolution.dto";

// Import local types to define the main Event DTO Map
import type { EmulatorConnectionStatusChangedDTO } from "./dto/connection.dto";
import type { StateDTO } from "./dto/state.dto";
import type { PlayerDTO } from "./dto/player.dto";
import type { ImportantItemsDTO } from "./dto/important-items.dto";
import type { PartyDTO } from "./dto/party.dto";
import type { BattleDTO } from "./dto/battle.dto";
import type { CardBattleDTO } from "./dto/card-battle.dto";
import type { JournalDTO } from "./dto/journal.dto";

// Mapeamento Estrito dos Eventos do SignalR e do Cliente
export interface EventsMap {
  EmulatorConnectionStatusChanged: EmulatorConnectionStatusChangedDTO;
  InitialState: StateDTO;
  PlayerChanged: PlayerDTO;
  ImportantItemsChanged: ImportantItemsDTO;
  PartyChanged: PartyDTO;
  BattleChanged: BattleDTO;
  CardBattleChanged: CardBattleDTO;
  JournalChanged: JournalDTO;
  HubConnectionStatusChanged: {
    isConnected: boolean;
    errorMessage?: string;
    preserveGameState?: boolean;
  };
}
