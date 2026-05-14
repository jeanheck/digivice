export interface Digimon {
    slotIndex: number;
    basicInfo: BasicInfo;
    attributes: Attributes;
    resistances: Resistances;
    equipments: Equipments;
    equippedDigievolutions: (Digievolution | null)[];
    activeDigievolutionId: number | null;
}

export interface BasicInfo {
    name: string;
    level: number;
    experience: number;
    currentHP: number;
    maxHP: number;
    currentMP: number;
    maxMP: number;
    experienceToReachNextLevel: number;
    experiencePercentageToReachNextLevel: number;
}

export enum DigimonStatusType {
    strength = "strength",
    defense = "defense",
    spirit = "spirit",
    wisdom = "wisdom",
    speed = "speed",
    charisma = "charisma",
    fire = "fire",
    water = "water",
    ice = "ice",
    wind = "wind",
    thunder = "thunder",
    machine = "machine",
    dark = "dark",
}

export interface DigimonStatus {
    digimonStatusType: DigimonStatusType;
    fromDigimon: number;
    fromEquipaments: number;
    fromDigievolution: number;
    sumBetweenDigimonAndEquipaments: number;
}

export interface Attributes {
    strength: DigimonStatus;
    defense: DigimonStatus;
    spirit: DigimonStatus;
    wisdom: DigimonStatus;
    speed: DigimonStatus;
    charisma: DigimonStatus;
}

export interface Resistances {
    fire: DigimonStatus;
    water: DigimonStatus;
    ice: DigimonStatus;
    wind: DigimonStatus;
    thunder: DigimonStatus;
    machine: DigimonStatus;
    dark: DigimonStatus;
}

export enum EquipamentType {
    WeaponSingleHanded = "WeaponSingleHanded",
    BodyArmor = "BodyArmor",
    WeaponTwoHanded = "WeaponTwoHanded",
    Shield = "Shield",
    Head = "Head",
    Accessory = "Accessory",
    Unknown = "Unknown",
    Empty = "Empty"
}

export enum EquipamentsAttributesOperationType {
    Addition = "Addition",
    Subtraction = "Subtraction"
}

export interface EquipamentsAttributes {
    attribute: DigimonStatusType;
    type: EquipamentsAttributesOperationType;
    value: number;
}

export interface Equipament {
    id: number;
    name: Record<string, string>;
    type: EquipamentType;
    typeDescription: Record<string, string> | null;
    attributes: EquipamentsAttributes[];
    equipableDigimon: string[];
    note?: Record<string, string>;
}

export interface Equipments {
    head: Equipament | null;
    body: Equipament | null;
    rightHand: Equipament | null;
    leftHand: Equipament | null;
    accessory1: Equipament | null;
    accessory2: Equipament | null;
}

export interface Digievolution {
    id: number;
    level: number;
    name: string; // Enriched via DigievolutionRegistry
}