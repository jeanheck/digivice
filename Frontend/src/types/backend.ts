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
    strength: number
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
    machine: number
    dark: number
}

export interface Equipments {
    head: number
    body: number
    rightHand: number
    leftHand: number
    accessory1: number
    accessory2: number
}
export interface Digievolution {
    id: number
    level: number
}

export interface Digimon {
    slotIndex: number
    basicInfo: BasicInfo
    attributes: Attributes
    resistances: Resistances
    equipments: Equipments
    equippedDigievolutions: (Digievolution | null)[]
}

export interface Party {
    slots: (Digimon | null)[]
}

export interface Player {
    name: string
    bits: number
}

export interface QuestStep {
    number: number
    description: string
    isCompleted: boolean
}

export interface Quest {
    id: string
    title: string
    description: string
    requirements: string[]
    steps: QuestStep[]
    done: boolean
    available: boolean
}

export interface MainQuest extends Quest { }
export interface SideQuest extends Quest { }

export interface Journal {
    mainQuest: MainQuest | null
    sideQuests: SideQuest[]
}

export interface State {
    player: Player | null
    party: Party | null
    importantItems?: Record<string, boolean>
    journal?: Journal | null
}
