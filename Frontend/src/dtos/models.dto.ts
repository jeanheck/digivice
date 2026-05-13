/**
 * Data Transfer Objects (DTOs)
 * These interfaces reflect the EXACT structure of the C# models sent via SignalR.
 * They follow the camelCase naming convention enforced by the SignalR JSON serializer.
 */

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
    equippedDigievolutions: (DigievolutionDTO | null)[];
    activeDigievolutionId: number | null;
}

export interface PartyDTO {
    slots: (DigimonDTO | null)[];
}

export interface PlayerDTO {
    name: string;
    bits: number;
    mapId: string;
}

export interface RequisiteDTO {
    description: string;
    isDone: boolean;
}

export interface QuestStepDTO {
    number: number;
    isCompleted: boolean;
    prerequisites: RequisiteDTO[] | null;
}

export interface QuestDTO {
    id: string | null;
    title: string;
    description: string;
    prerequisites: RequisiteDTO[];
    steps: QuestStepDTO[];
}

export interface JournalDTO {
    mainQuest: QuestDTO | null;
    sideQuests: QuestDTO[];
}

export interface ImportantItemDTO {
    id: string;
    name: string;
    has: boolean;
}

export interface ConsumableItemDTO {
    id: string;
    name: string;
    quantity: number;
}

export interface ImportantItemsDTO {
    folderBag: ImportantItemDTO | null;
    treeBoots: ImportantItemDTO | null;
    fishingPole: ImportantItemDTO | null;
    redSnapper: ImportantItemDTO | null;
}

export interface ConsumableItemsDTO {
    powerCharge: ConsumableItemDTO | null;
    spiderWeb: ConsumableItemDTO | null;
    bambooSpear: ConsumableItemDTO | null;
}

export interface StateDTO {
    player: PlayerDTO | null;
    party: PartyDTO | null;
    importantItems: ImportantItemsDTO | null;
    consumableItems: ConsumableItemsDTO | null;
    journal: JournalDTO | null;
}
