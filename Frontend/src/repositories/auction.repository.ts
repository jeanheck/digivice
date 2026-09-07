import AuctionsJson from "@/database/auction/auctions.json";
import type { AuctionTable } from "@/repositories/tables/auction/auction.table";
import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";

export class AuctionRepository {
  private static readonly auctionTable = AuctionsJson as AuctionTable;

  public static getAuctions(): AuctionRaw[] {
    return Object.entries(this.auctionTable).map(([id, entry]) => {
      return { ...entry, id };
    });
  }

  public static getAuctionById(auctionId: string): AuctionRaw | null {
    const entry = this.auctionTable[auctionId];
    if (entry === undefined) {
      return null;
    }

    return { ...entry, id: auctionId };
  }
}
