import type { AuctionStatus } from "@/types/auction-status.type";
import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class AuctionConverter {
  public static convert(auctionRaw: AuctionRaw, status: AuctionStatus): AuctionViewModel {
    return {
      id: auctionRaw.id,
      equipmentId: Number(auctionRaw.equipmentId),
      status,
      price: auctionRaw.price,
      resale: auctionRaw.resale,
    };
  }
}
