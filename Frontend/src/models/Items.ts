export interface Item {
    id: string;
    name: string;
}

export interface ImportantItem extends Item {
    has: boolean;
}

export interface ImportantItems {
    folderBag: ImportantItem;
    treeBoots: ImportantItem;
    fishingPole: ImportantItem;
    redSnapper: ImportantItem;
}
