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
    folderBag: ImportantItem;
    treeBoots: ImportantItem;
    fishingPole: ImportantItem;
    redSnapper: ImportantItem;
}

export interface ConsumableItems {
    powerCharge: ConsumableItem;
    spiderWeb: ConsumableItem;
    bambooSpear: ConsumableItem;
}
