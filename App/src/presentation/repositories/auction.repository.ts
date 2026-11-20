import AuctionJson from "@/data/game/auction.json";
import type { AuctionTable } from "@/presentation/repositories/tables/auction/auction.table";
import type { AuctionRaw } from "@/presentation/repositories/tables/raws/auction/auction.raw";

export class AuctionRepository {
  private static readonly auctionTable = AuctionJson as AuctionTable;

  public static getAuctions(): AuctionRaw[] {
    return this.auctionTable;
  }
}
