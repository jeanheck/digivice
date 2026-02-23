export interface BasicInfo {
    name: string
    level: number
    experience: number
    currentHP: number
    maxHP: number
    currentMP: number
    maxMP: number
}

export interface Attributes {
    attack: number
    defense: number
    spirit: number
    wisdom: number
    speed: number
    charisma: number
}

export interface Resistances {
    fire: number
    water: number
    ice: number
    wind: number
    thunder: number
    metal: number
    dark: number
}

export interface Digimon {
    slotIndex: number
    basicInfo: BasicInfo
    attributes: Attributes
    resistances: Resistances
}

export interface Party {
    slots: (Digimon | null)[]
    activeSlotIndex: number
}

export interface Player {
    name: string
    bits: number
}

export interface State {
    player: Player | null
    party: Party | null
}
