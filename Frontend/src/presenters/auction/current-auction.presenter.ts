import type { Auctions, Quest } from "@/models";
import { AuctionService } from "@/services/auction.service";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class CurrentAuctionPresenter {
  public static getAuctionAvailable(
    auctions: Auctions | null,
    mainQuest: Quest | null,
  ): AuctionViewModel | null {
    return AuctionService.getAvailableAuction(auctions, mainQuest);
  }
}
