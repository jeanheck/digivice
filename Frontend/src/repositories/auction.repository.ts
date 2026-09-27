import AuctionJson from "@/database/auction.json";
import type { AuctionTable } from "@/repositories/tables/auction/auction.table";
import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";

export class AuctionRepository {
  private static readonly auctionTable = AuctionJson as AuctionTable;

  public static getAuctions(): AuctionRaw[] {
    return this.auctionTable;
  }
}
