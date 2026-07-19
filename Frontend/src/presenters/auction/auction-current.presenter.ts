import type { Journal } from "@/models";
import { AuctionConverter } from "@/presenters/converter/auction.converter";
import { AuctionRepository } from "@/repositories/auction.repository";
import { AuctionService } from "@/services/auction.service";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";

export class AuctionCurrentPresenter {
    public static getAuctionCurrentViewModel(journal: Journal | null): AuctionCurrentViewModel {
        const auctionAvailableNow = AuctionService.getAuctionAvailableNow(journal);

        if (auctionAvailableNow === null) {
            return AuctionConverter.convertCurrent(null);
        }

        const auctionRaw = AuctionRepository.getAuctionById(auctionAvailableNow.id);
        return AuctionConverter.convertCurrent(auctionRaw);
    }
}
