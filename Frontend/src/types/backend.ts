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
    activeDigievolutionId: number | null
}

export interface Party {
    slots: (Digimon | null)[]
}

export interface Player {
    name: string;
    bits: number;
    mapId: string;
}

export interface Requisite {
    description: string
    isDone: boolean
}

export interface MapCoordinates {
    x: number;
    y: number;
}

export interface StepLocation {
    locationImage?: string;
    target?: string;
    locationImageCoordinates?: MapCoordinates;
}

export interface QuestStep {
    number: number
    description?: string
    isCompleted: boolean
    prerequisites?: Requisite[]
    locationOnMap?: string;
    locationOnMapCoordinates?: MapCoordinates;
    locations?: StepLocation[];
}

export interface Quest {
    Id: string
    id: number
    title: string
    description?: string
    prerequisites: Requisite[]
    steps: QuestStep[]
}

export interface MainQuest extends Quest { }
export interface SideQuest extends Quest { }

export interface Journal {
    mainQuest: MainQuest
    sideQuests: SideQuest[]
}

export interface IItem {
    id: string;
    name: string;
}

export interface ImportantItem extends IItem {
    has: boolean;
}

export interface ConsumableItem extends IItem {
    quantity: number;
}

export interface ImportantItems {
    folderBag: ImportantItem | null;
    treeBoots: ImportantItem | null;
    fishingPole: ImportantItem | null;
    redSnapper: ImportantItem | null;
}

export interface ConsumableItems {
    powerCharge: ConsumableItem | null;
    spiderWeb: ConsumableItem | null;
    bambooSpear: ConsumableItem | null;
}

export interface State {
    player: Player | null;
    party: Party | null;
    importantItems: ImportantItems | null;
    consumableItems: ConsumableItems | null;
    journal: Journal | null;
}
