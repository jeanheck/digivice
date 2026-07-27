import type { Journal } from "@/models";
import { AuctionService } from "@/services/auction.service";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class AuctionModalPresenter {
    public static getAuctions(journal: Journal | null): AuctionViewModel[] {
        return AuctionService.getAuctions(journal);
    }
}
