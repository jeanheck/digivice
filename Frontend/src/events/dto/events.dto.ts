// Export everything from individual modular files
export type { PlayerDTO } from './PlayerDTO';
export type { ConnectionDTO, ConnectionStatusChangedDTO } from './ConnectionDTO';
export type { PartyDTO } from './PartyDTO';
export type { JournalDTO } from './JournalDTO';
export type { StateDTO } from './StateDTO';

export type { QuestDTO } from './Journals/QuestDTO';
export type { RequisiteDTO } from './Journals/Quests/RequisiteDTO';
export type { StepDTO } from './Journals/Quests/StepDTO';

export type { DigimonDTO } from './Parties/DigimonDTO';
export type { DigimonSlotDTO } from './Parties/DigimonSlotDTO';
export type { VitalsDTO } from './Parties/Digimons/VitalsDTO';
export type { AttributesDTO } from './Parties/Digimons/AttributesDTO';
export type { ResistancesDTO } from './Parties/Digimons/ResistancesDTO';
export type { EquipmentsDTO } from './Parties/Digimons/EquipmentsDTO';
export type { DigievolutionDTO } from './Parties/Digimons/DigievolutionDTO';
export type { DigievolutionSlotDTO } from './Parties/Digimons/DigievolutionSlotDTO';

// Import local types to define the main Event DTO Map
import type { ConnectionStatusChangedDTO } from './ConnectionDTO';
import type { StateDTO } from './StateDTO';
import type { PlayerDTO } from './PlayerDTO';
import type { PartyDTO } from './PartyDTO';
import type { JournalDTO } from './JournalDTO';
import type { DigimonDTO } from './Parties/DigimonDTO';
import type { DigievolutionSlotDTO } from './Parties/Digimons/DigievolutionSlotDTO';
import type { EquipmentsDTO } from './Parties/Digimons/EquipmentsDTO';

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
