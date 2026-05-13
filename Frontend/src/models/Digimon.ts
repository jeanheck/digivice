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
}

export interface Attributes {
    strength: number;
    defense: number;
    spirit: number;
    wisdom: number;
    speed: number;
    charisma: number;
}

export interface Resistances {
    fire: number;
    water: number;
    ice: number;
    wind: number;
    thunder: number;
    machine: number;
    dark: number;
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
