import type { Digimon, State, ImportantItems, Journal } from './backend'

// --- Player Events ---
export interface ConnectionStatusChanged {
    isConnected: boolean
}

export interface InitialStateChanged {
    initialState: State
}

export interface PlayerBitsChanged {
    newBits: number
}

export interface PlayerNameChanged {
    newName: string
}

export interface PlayerLocationChanged {
    location: string
}

// --- Party & Digimon Events ---
export interface PartySlotsChanged {
    newParty: (Digimon | null)[]
}

export interface DigimonVitalsChanged {
    partySlotIndex: number
    currentHP: number
    maxHP: number
    currentMP: number
    maxMP: number
}

export interface DigimonExperienceChanged {
    partySlotIndex: number
    level: number
    currentEXP: number
}

export interface DigimonLevelChanged {
    partySlotIndex: number
    oldLevel: number
    newLevel: number
}

export interface DigimonAttributesChanged {
    partySlotIndex: number
    strength: number
    defense: number
    spirit: number
    wisdom: number
    speed: number
    charisma: number
}

export interface DigimonResistancesChanged {
    partySlotIndex: number
    fire: number
    water: number
    ice: number
    wind: number
    thunder: number
    machine: number
    dark: number
}

export interface DigimonEquipmentsChanged {
    partySlotIndex: number
    equipments: Digimon['equipments']
}

export interface DigimonDigievolutionsChanged {
    partySlotIndex: number
    equippedDigievolutions: Digimon['equippedDigievolutions']
}

export interface DigimonActiveDigievolutionChanged {
    partySlotIndex: number
    activeDigievolutionId: number | null
}

// --- Items & Journal ---
export interface ImportantItemsChanged {
    importantItems: ImportantItems | null
}

export interface JournalChanged {
    journal: Journal | null
}

/**
 * Master Map of all SignalR events and their respective payloads.
 * This ensures end-to-end type safety between Backend and Frontend.
 */
export interface GameEventMap {
    ConnectionStatusChanged: ConnectionStatusChanged
    InitialStateChanged: InitialStateChanged
    PlayerBitsChanged: PlayerBitsChanged
    PlayerNameChanged: PlayerNameChanged
    PlayerLocationChanged: PlayerLocationChanged
    PartySlotsChanged: PartySlotsChanged
    DigimonVitalsChanged: DigimonVitalsChanged
    DigimonExperienceChanged: DigimonExperienceChanged
    DigimonLevelChanged: DigimonLevelChanged
    DigimonAttributesChanged: DigimonAttributesChanged
    DigimonResistancesChanged: DigimonResistancesChanged
    DigimonEquipmentsChanged: DigimonEquipmentsChanged
    DigimonDigievolutionsChanged: DigimonDigievolutionsChanged
    DigimonActiveDigievolutionChanged: DigimonActiveDigievolutionChanged
    ImportantItemsChanged: ImportantItemsChanged
    JournalChanged: JournalChanged
}
