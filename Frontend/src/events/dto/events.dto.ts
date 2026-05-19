// Export everything from individual modular files
export type { PlayerDTO } from './player.dto';
export type { ConnectionDTO, ConnectionStatusChangedDTO } from './connection.dto';
export type { PartyDTO } from './party.dto';
export type { JournalDTO } from './journal.dto';
export type { StateDTO } from './state.dto';

export type { QuestDTO } from './journals/quest.dto';
export type { RequisiteDTO } from './journals/quests/requisite.dto';
export type { StepDTO } from './journals/quests/step.dto';

export type { DigimonDTO } from './parties/digimon.dto';
export type { DigimonSlotDTO } from './parties/digimon-slot.dto';
export type { VitalsDTO } from './parties/digimons/vitals.dto';
export type { AttributesDTO } from './parties/digimons/attributes.dto';
export type { ResistancesDTO } from './parties/digimons/resistances.dto';
export type { EquipmentsDTO } from './parties/digimons/equipments.dto';
export type { DigievolutionDTO } from './parties/digimons/digievolution.dto';
export type { DigievolutionSlotDTO } from './parties/digimons/digievolution-slot.dto';

// Import local types to define the main Event DTO Map
import type { ConnectionStatusChangedDTO } from './connection.dto';
import type { StateDTO } from './state.dto';
import type { PlayerDTO } from './player.dto';
import type { PartyDTO } from './party.dto';
import type { JournalDTO } from './journal.dto';
import type { DigimonDTO } from './parties/digimon.dto';
import type { DigievolutionSlotDTO } from './parties/digimons/digievolution-slot.dto';
import type { EquipmentsDTO } from './parties/digimons/equipments.dto';

// Mapeamento Estrito dos 5 Eventos do SignalR
export interface GameEventDTOMap {
    ConnectionStatusChanged: ConnectionStatusChangedDTO;
    InitialState: StateDTO;
    PlayerChanged: PlayerDTO;
    PartyChanged: PartyDTO;
    JournalChanged: JournalDTO;
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
