import type { AuctionDTO } from "./auctions/auction.dto";
import type { QuestDTO } from "./journals/quest.dto";

export interface JournalDTO {
    mainQuest?: QuestDTO | null;
    sideQuests?: QuestDTO[];
    legendaryWeapons?: QuestDTO[];
    driAgents?: QuestDTO[];
    auctions?: AuctionDTO[];
}
