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

export interface Equipments {
    head: number;
    body: number;
    rightHand: number;
    leftHand: number;
    accessory1: number;
    accessory2: number;
}

export interface Digievolution {
    id: number;
    level: number;
    name: string; // Enriched via DigievolutionRegistry
}