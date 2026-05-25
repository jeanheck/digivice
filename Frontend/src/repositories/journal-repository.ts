import type { MainQuestTable } from "./tables/quest/main-quest-table";
import type { TreeBootsTable } from "./tables/quest/tree-boots-table";
import type { FishingPoleTable } from "./tables/quest/fishing-pole-table";
import type { FolderBagTable } from "./tables/quest/folder-bag-table";
import type { MainQuestRaw } from "./tables/raws/quest/main-quest-raw";
import MainQuestJson from "@/database/quest/main-quest.json";
import TreeBootsJson from "@/database/quest/side-quest/tree-boots.json";
import FishingPoleJson from "@/database/quest/side-quest/fishing-pole.json";
import FolderBagJson from "@/database/quest/side-quest/folder-bag.json";

export class JournalRepository {
    private static readonly mainQuestTable = MainQuestJson as MainQuestTable;
    private static readonly treeBootsTable = TreeBootsJson as TreeBootsTable;
    private static readonly fishingPoleTable = FishingPoleJson as FishingPoleTable;
    private static readonly folderBagTable = FolderBagJson as FolderBagTable;

    public static getMainQuestRaw(): MainQuestRaw | null {
        return null;
    }
}
