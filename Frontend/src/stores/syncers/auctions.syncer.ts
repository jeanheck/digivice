import type { Auctions } from "@/models/auctions";
import type * as Events from "@/events/events.map";

export class AuctionsSyncer {
  public static sync(previousAuctions: Auctions, newAuctionsDto: Events.AuctionsDTO): void {
    if (newAuctionsDto.divineBarrier !== undefined) {
      previousAuctions.divineBarrier = newAuctionsDto.divineBarrier;
    }
    if (newAuctionsDto.hazardShield !== undefined) {
      previousAuctions.hazardShield = newAuctionsDto.hazardShield;
    }
    if (newAuctionsDto.sniperShield !== undefined) {
      previousAuctions.sniperShield = newAuctionsDto.sniperShield;
    }
    if (newAuctionsDto.dramonShield !== undefined) {
      previousAuctions.dramonShield = newAuctionsDto.dramonShield;
    }
    if (newAuctionsDto.yinYangWand !== undefined) {
      previousAuctions.yinYangWand = newAuctionsDto.yinYangWand;
    }
  }
}
