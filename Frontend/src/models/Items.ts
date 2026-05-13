export interface Item {
    id: string;
    name: string;
}

export interface ImportantItem extends Item {
    has: boolean;
}

export interface ConsumableItem extends Item {
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
