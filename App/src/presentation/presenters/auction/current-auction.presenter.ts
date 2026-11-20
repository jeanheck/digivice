import type { Auctions, Quest } from "@/state/models";
import { AuctionService } from "@/presentation/services/auction.service";
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

export class CurrentAuctionPresenter {
  public static getAvailableAuction(auctions: Auctions, mainQuest: Quest): AuctionViewModel | null {
    return AuctionService.getAvailableAuction(auctions, mainQuest);
  }

  public static hasNotEnoughBits(auction: AuctionViewModel, playerBits: number): boolean {
    return auction.bid > playerBits;
  }
}
