// Export everything from individual modular files in dto folder
export type { PlayerDTO } from './dto/player.dto';
export type { ConnectionDTO, EmulatorConnectionStatusChangedDTO } from './dto/connection.dto';
export type { PartyDTO } from './dto/party.dto';
export type { JournalDTO } from './dto/journal.dto';
export type { StateDTO } from './dto/state.dto';

export type { QuestDTO } from './dto/journals/quest.dto';
export type { RequisiteDTO } from './dto/journals/quests/requisite.dto';
export type { StepDTO } from './dto/journals/quests/step.dto';

export type { DigimonDTO } from './dto/parties/digimon.dto';
export type { DigimonSlotDTO } from './dto/parties/digimon-slot.dto';
export type { VitalsDTO } from './dto/parties/digimons/vitals.dto';
export type { AttributesDTO } from './dto/parties/digimons/attributes.dto';
export type { ResistancesDTO } from './dto/parties/digimons/resistances.dto';
export type { EquipmentsDTO } from './dto/parties/digimons/equipments.dto';
export type { DigievolutionDTO } from './dto/parties/digimons/digievolution.dto';
export type { DigievolutionSlotDTO } from './dto/parties/digimons/digievolution-slot.dto';

// Import local types to define the main Event DTO Map
import type { EmulatorConnectionStatusChangedDTO } from './dto/connection.dto';
import type { StateDTO } from './dto/state.dto';
import type { PlayerDTO } from './dto/player.dto';
import type { PartyDTO } from './dto/party.dto';
import type { JournalDTO } from './dto/journal.dto';
import type { DigimonDTO } from './dto/parties/digimon.dto';
import type { EquipmentsDTO } from './dto/parties/digimons/equipments.dto';
import type { DigievolutionSlotDTO } from './dto/parties/digimons/digievolution-slot.dto';

// Mapeamento Estrito dos Eventos do SignalR e do Cliente
export interface EventsMap {
    EmulatorConnectionStatusChanged: EmulatorConnectionStatusChangedDTO;
    InitialState: StateDTO;
    PlayerChanged: PlayerDTO;
    PartyChanged: PartyDTO;
    JournalChanged: JournalDTO;
    HubConnectionStatusChanged: { isConnected: boolean };
}

// ==========================================
// CAMADA DE COMPATIBILIDADE LEGADA (STORES)
// ==========================================

export interface ItemDTO {
    id: string;
    name: string;
}

export interface ImportantItemDTO extends ItemDTO {
    has: boolean;
}

export interface ImportantItemsDTO {
    folderBag: ImportantItemDTO;
    treeBoots: ImportantItemDTO;
    fishingPole: ImportantItemDTO;
    redSnapper: ImportantItemDTO;
}

export interface InitialStateChangedDTO {
    state: {
        player: PlayerDTO | null;
        party: PartyDTO | null;
        importantItems: ImportantItemsDTO | null;
        journal: JournalDTO | null;
    };
}

export interface PlayerBitsChangedDTO {
    bits: number;
}

export interface PlayerNameChangedDTO {
    name: string;
}

export interface PlayerLocationChangedDTO {
    location: string;
}

export interface PartySlotsChangedDTO {
    party: (DigimonDTO | null)[];
}

export interface DigimonVitalsChangedDTO {
    partySlotIndex: number;
    currentHP: number;
    maxHP: number;
    currentMP: number;
    maxMP: number;
}

export interface DigimonExperienceChangedDTO {
    partySlotIndex: number;
    experience: number;
}

export interface DigimonLevelChangedDTO {
    partySlotIndex: number;
    level: number;
}

export interface DigimonAttributesChangedDTO {
    partySlotIndex: number;
    strength: number;
    defense: number;
    spirit: number;
    wisdom: number;
    speed: number;
    charisma: number;
}

export interface DigimonResistancesChangedDTO {
    partySlotIndex: number;
    fire: number;
    water: number;
    ice: number;
    wind: number;
    thunder: number;
    machine: number;
    dark: number;
}

export interface DigimonEquipmentsChangedDTO {
    partySlotIndex: number;
    equipments: EquipmentsDTO;
}

export interface DigimonDigievolutionsChangedDTO {
    partySlotIndex: number;
    digievolutions: (DigievolutionSlotDTO[] | null);
}

export interface DigimonActiveDigievolutionChangedDTO {
    partySlotIndex: number;
    activeDigievolutionId: number | null;
}

export interface ImportantItemsChangedDTO {
    importantItems: ImportantItemsDTO | null;
}

export interface JournalChangedDTO {
    journal: JournalDTO | null;
}
