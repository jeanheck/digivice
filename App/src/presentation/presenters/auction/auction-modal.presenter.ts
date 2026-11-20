import type { Auctions, Quest } from "@/state/models";
import { AuctionService } from "@/presentation/services/auction.service";
import type { AuctionViewModel } from "@/presentation/viewmodels/auction/auction.viewmodel";

export class AuctionModalPresenter {
  public static getAuctions(auctions: Auctions, mainQuest: Quest): AuctionViewModel[] {
    return AuctionService.getAuctions(auctions, mainQuest);
  }
}
