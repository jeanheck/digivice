import type { Auctions, Quest } from "@/state/models";
import { AuctionService } from "@/presentation/services/auction.service";
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

export class AuctionCardPresenter {
  public static getAvailableAuction(auctions: Auctions, mainQuest: Quest): AuctionViewModel | null {
    return AuctionService.getAvailableAuction(auctions, mainQuest);
  }
}
