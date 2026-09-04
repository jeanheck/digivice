import type { Auctions, Journal } from "@/models";
import { AuctionService } from "@/services/auction.service";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class AuctionCardPresenter {
  public static getAuctionAvailable(
    auctions: Auctions | null,
    journal: Journal | null,
  ): AuctionViewModel | null {
    return AuctionService.getAuctionAvailable(auctions, journal);
  }
}
