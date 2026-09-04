import type { Auctions, Journal } from "@/models";
import { AuctionService } from "@/services/auction.service";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class AuctionModalPresenter {
  public static getAuctions(auctions: Auctions | null, journal: Journal | null): AuctionViewModel[] {
    return AuctionService.getAuctions(auctions, journal);
  }
}
