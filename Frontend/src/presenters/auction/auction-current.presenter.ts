import type { Journal } from "@/models";
import { AuctionRepository } from "@/repositories/auction.repository";
import { AuctionService } from "@/services/auction.service";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";
import { AuctionCurrentConverter } from "../converter/auction-current.converter";

export class AuctionCurrentPresenter {
    public static getAuctionCurrent(journal: Journal | null): AuctionCurrentViewModel | null {
        const auctionAvailableNow = AuctionService.getAuctionAvailableNow(journal);

        if (auctionAvailableNow === null) {
            return null;
        }

        const auctionRaw = AuctionRepository.getAuctionById(auctionAvailableNow.id);
        return AuctionCurrentConverter.convert(auctionRaw);
    }
}
