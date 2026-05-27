import MainQuestJson from "@/database/quest/main-quest.json";
import TreeBootsJson from "@/database/quest/side-quest/tree-boots.json";
import FishingPoleJson from "@/database/quest/side-quest/fishing-pole.json";
import FolderBagJson from "@/database/quest/side-quest/folder-bag.json";
import type { MainQuestTable } from "@/repositories/tables/quest/main-quest.table";
import type { TreeBootsTable } from "@/repositories/tables/quest/side-quest/tree-boots.table";
import type { FishingPoleTable } from "@/repositories/tables/quest/side-quest/fishing-pole.table";
import type { FolderBagTable } from "@/repositories/tables/quest/side-quest/folder-bag.table";
import type { QuestRaw } from "@/repositories/tables/raws/quest/quest.raw";

export class QuestRepository {
    private static readonly mainQuestTable = MainQuestJson as MainQuestTable;
    private static readonly treeBootsTable = TreeBootsJson as TreeBootsTable;
    private static readonly fishingPoleTable = FishingPoleJson as FishingPoleTable;
    private static readonly folderBagTable = FolderBagJson as FolderBagTable;

    public static getMainQuestRaw(): QuestRaw {
        return this.mainQuestTable;
    }
    public static getSideQuestsRaw(): QuestRaw[] {
        return [this.folderBagTable, this.fishingPoleTable, this.treeBootsTable];
    }
}
