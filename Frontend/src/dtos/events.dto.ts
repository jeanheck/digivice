// Sub-DTOs de Digimon
export interface VitalsDTO {
    maxHP?: number;
    maxMP?: number;
    currentHP?: number;
    currentMP?: number;
}

export interface AttributesDTO {
    strength?: number;
    defense?: number;
    spirit?: number;
    wisdom?: number;
    speed?: number;
    charisma?: number;
}

export interface ResistancesDTO {
    fire?: number;
    water?: number;
    ice?: number;
    wind?: number;
    thunder?: number;
    machine?: number;
    dark?: number;
}

export interface EquipmentsDTO {
    head?: number;
    body?: number;
    rightHand?: number;
    leftHand?: number;
    accessory1?: number;
    accessory2?: number;
}

export interface DigievolutionDTO {
    level?: number;
}

export interface DigievolutionSlotDTO {
    index: number;
    digievolutionId?: number;
    digievolution?: DigievolutionDTO | null;
}

// DTO principal de Digimon e seu Slot
export interface DigimonDTO {
    level?: number;
    experience?: number;
    vitals?: VitalsDTO;
    attributes?: AttributesDTO;
    resistances?: ResistancesDTO;
    equipments?: EquipmentsDTO;
    digievolutions?: DigievolutionSlotDTO[];
    activeDigievolutionId?: number;
}

export interface DigimonSlotDTO {
    index: number;
    digimonId?: number | null;
    digimon?: DigimonDTO | null;
}

// DTOs Principais
export interface PlayerDTO {
    name?: string;
    bits?: number;
    location?: string; // Corresponde ao MapId no backend
    mapId?: string;    // Legacy fallback
}

export interface PartyDTO {
    slots?: DigimonSlotDTO[];
}

// Diário e Missões
export interface RequisiteDTO {
    id: string;
    value?: number; // Representa o byte bruto
}

export interface StepDTO {
    number: number;
    value?: number; // Representa o byte bruto
    requisites?: RequisiteDTO[];
}

export interface QuestDTO {
    id: string;
    requisites?: RequisiteDTO[];
    steps?: StepDTO[];
}

export interface JournalDTO {
    mainQuest?: QuestDTO | null;
    sideQuests?: QuestDTO[];
}

// Estado Inicial e Status de Conexão
export interface StateDTO {
    player: PlayerDTO | null;
    party: PartyDTO | null;
    journal: JournalDTO | null;
}

export interface ConnectionStatusChangedDTO {
    isConnected: boolean;
}

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
