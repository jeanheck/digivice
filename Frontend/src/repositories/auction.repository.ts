import AuctionsJson from "@/database/auction/auctions.json";
import type { AuctionTable } from "@/repositories/tables/auction/auction.table";
import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";

export class AuctionRepository {
  private static readonly auctionTable = AuctionsJson as AuctionTable;

  public static getAuctions(): AuctionRaw[] {
    return this.auctionTable;
  }

  public static getAuctionById(auctionId: string): AuctionRaw | null {
    return (
      this.auctionTable.find((auction) => {
        return auction.id === auctionId;
      }) ?? null
    );
  }
}
