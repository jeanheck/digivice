import type { Journal } from "@/models";
import { AuctionService } from "@/services/auction.service";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionModalPresenter {
    public static getAuctions(journal: Journal | null): AuctionListItemViewModel[] {
        return AuctionService.getAuctions(journal);
    }
}
