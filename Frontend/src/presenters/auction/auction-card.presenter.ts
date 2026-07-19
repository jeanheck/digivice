import type { Journal } from "@/models";
import { AuctionService } from "@/services/auction.service";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionCardPresenter {
    public static getAuctionAvailableNow(journal: Journal | null): AuctionListItemViewModel | null {
        return AuctionService.getAuctionAvailableNow(journal);
    }
}
