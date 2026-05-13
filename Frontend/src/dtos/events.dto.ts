import type { 
    DigimonDTO, 
    StateDTO, 
    ImportantItemsDTO, 
    JournalDTO,
    AttributesDTO,
    ResistancesDTO,
    EquipmentsDTO,
    DigievolutionDTO
} from './models.dto';

// --- Player Events ---
export interface ConnectionStatusChangedDTO {
    isConnected: boolean;
}

export interface InitialStateChangedDTO {
    initialState: StateDTO;
}

export interface PlayerBitsChangedDTO {
    newBits: number;
}

export interface PlayerNameChangedDTO {
    newName: string;
}

export interface PlayerLocationChangedDTO {
    location: string;
}

// --- Party & Digimon Events ---
export interface PartySlotsChangedDTO {
    newParty: (DigimonDTO | null)[];
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
    level: number;
    currentEXP: number;
}

export interface DigimonLevelChangedDTO {
    partySlotIndex: number;
    oldLevel: number;
    newLevel: number;
}

export interface DigimonAttributesChangedDTO extends AttributesDTO {
    partySlotIndex: number;
}

export interface DigimonResistancesChangedDTO extends ResistancesDTO {
    partySlotIndex: number;
}

export interface DigimonEquipmentsChangedDTO {
    partySlotIndex: number;
    equipments: EquipmentsDTO;
}

export interface DigimonDigievolutionsChangedDTO {
    partySlotIndex: number;
    equippedDigievolutions: (DigievolutionDTO | null)[];
}

export interface DigimonActiveDigievolutionChangedDTO {
    partySlotIndex: number;
    activeDigievolutionId: number | null;
}

// --- Items & Journal ---
export interface ImportantItemsChangedDTO {
    importantItems: ImportantItemsDTO | null;
}

export interface JournalChangedDTO {
    journal: JournalDTO | null;
}

/**
 * Master Map of all SignalR events and their respective DTO payloads.
 */
export interface GameEventDTOMap {
    ConnectionStatusChanged: ConnectionStatusChangedDTO;
    InitialStateChanged: InitialStateChangedDTO;
    PlayerBitsChanged: PlayerBitsChangedDTO;
    PlayerNameChanged: PlayerNameChangedDTO;
    PlayerLocationChanged: PlayerLocationChangedDTO;
    PartySlotsChanged: PartySlotsChangedDTO;
    DigimonVitalsChanged: DigimonVitalsChangedDTO;
    DigimonExperienceChanged: DigimonExperienceChangedDTO;
    DigimonLevelChanged: DigimonLevelChangedDTO;
    DigimonAttributesChanged: DigimonAttributesChangedDTO;
    DigimonResistancesChanged: DigimonResistancesChangedDTO;
    DigimonEquipmentsChanged: DigimonEquipmentsChangedDTO;
    DigimonDigievolutionsChanged: DigimonDigievolutionsChangedDTO;
    DigimonActiveDigievolutionChanged: DigimonActiveDigievolutionChangedDTO;
    ImportantItemsChanged: ImportantItemsChangedDTO;
    JournalChanged: JournalChangedDTO;
}
