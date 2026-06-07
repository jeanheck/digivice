import type { Auction } from "./auction";
import type { Quest } from "./quest";

export interface Journal {
    mainQuest: Quest | null;
    sideQuests: Quest[];
    legendaryWeapons: Quest[];
    driAgents: Quest[];
    auctions: Auction[];
}
