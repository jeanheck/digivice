import type { Auctions, Quest } from "@/models";
import { AuctionService } from "@/services/auction.service";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class AuctionModalPresenter {
  public static getAuctions(auctions: Auctions | null, mainQuest: Quest | null): AuctionViewModel[] {
    return AuctionService.getAuctions(auctions, mainQuest);
  }
}
