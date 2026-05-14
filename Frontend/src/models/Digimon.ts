/**
 * Domain Model for Digimon
 * This represents a "rich" Digimon entity used by the UI.
 * Unlike the DTO, this includes data merged from static registries.
 */

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

export interface DigimonStatus {
    type: string;
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

export interface Digimon {
    slotIndex: number;
    basicInfo: BasicInfo;
    attributes: Attributes;
    resistances: Resistances;
    equipments: Equipments;
    equippedDigievolutions: (Digievolution | null)[];
    activeDigievolutionId: number | null;
}
