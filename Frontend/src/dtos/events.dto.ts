export interface BasicInfoDTO {
    name: string;
    level: number;
    experience: number;
    currentHP: number;
    maxHP: number;
    currentMP: number;
    maxMP: number;
}

export interface AttributesDTO {
    strength: number;
    defense: number;
    spirit: number;
    wisdom: number;
    speed: number;
    charisma: number;
}

export interface ResistancesDTO {
    fire: number;
    water: number;
    ice: number;
    wind: number;
    thunder: number;
    machine: number;
    dark: number;
}

export interface EquipmentsDTO {
    head: number;
    body: number;
    rightHand: number;
    leftHand: number;
    accessory1: number;
    accessory2: number;
}

export interface DigievolutionDTO {
    id: number;
    level: number;
}

export interface DigimonDTO {
    slotIndex: number;
    basicInfo: BasicInfoDTO;
    attributes: AttributesDTO;
    resistances: ResistancesDTO;
    equipments: EquipmentsDTO;
    activeDigievolutionId: number | null;
    equippedDigievolutions: (DigievolutionDTO | null)[];
}

export interface PlayerDTO {
    name: string;
    bits: number;
    mapId: string;
}

export interface PartyDTO {
    slots: (DigimonDTO | null)[];
}

export interface ItemDTO {
    id: string;
    name: string;
}

export interface ImportantItemDTO extends ItemDTO {
    has: boolean;
}

export interface ConsumableItemDTO extends ItemDTO {
    quantity: number;
}

export interface ImportantItemsDTO {
    folderBag: ImportantItemDTO;
    treeBoots: ImportantItemDTO;
    fishingPole: ImportantItemDTO;
    redSnapper: ImportantItemDTO;
}

export interface ConsumableItemsDTO {
    powerCharge: ConsumableItemDTO;
    spiderWeb: ConsumableItemDTO;
    bambooSpear: ConsumableItemDTO;
}

export interface RequisiteDTO {
    description: string;
    isDone: boolean;
    itemKey?: string;
}

export interface QuestStepDTO {
    number: number;
    isCompleted: boolean;
    prerequisites?: RequisiteDTO[];
}

export interface QuestDTO {
    id: string;
    title: string;
    description: string;
    prerequisites: RequisiteDTO[];
    steps: QuestStepDTO[];
}

export interface JournalDTO {
    mainQuest: QuestDTO | null;
    sideQuests: QuestDTO[];
}

export interface StateDTO {
    player: PlayerDTO | null;
    party: PartyDTO | null;
    importantItems: ImportantItemsDTO | null;
    consumableItems: ConsumableItemsDTO | null;
    journal: JournalDTO | null;
}

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
    equippedDigievolutions: (DigievolutionDTO | null)[];
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
