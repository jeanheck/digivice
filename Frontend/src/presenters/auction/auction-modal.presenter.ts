import type { Journal } from "@/models";
import { AuctionConverter } from "@/presenters/converter/auction.converter";
import { AuctionRepository } from "@/repositories/auction.repository";
import { AuctionService } from "@/services/auction.service";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionModalPresenter {
    public static getAuctionCurrentViewModel(journal: Journal | null): AuctionCurrentViewModel {
        const auctionAvailableNow = AuctionService.getAuctionAvailableNow(journal);

        if (auctionAvailableNow === null) {
            return AuctionConverter.convertCurrent(null);
        }

        const auctionRaw = AuctionRepository.getAuctionById(auctionAvailableNow.id);
        return AuctionConverter.convertCurrent(auctionRaw);
    }

    public static getAuctions(journal: Journal | null): AuctionListItemViewModel[] {
        return AuctionService.getAuctions(journal);
    }
}
