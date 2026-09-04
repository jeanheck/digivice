import type { AuctionsDTO } from "@/events/dto/auctions.dto";
import type { Auctions } from "@/models/auctions";

export class AuctionsConverter {
  public static convert(auctionsDto: Required<AuctionsDTO>): Auctions {
    return {
      divineBarrier: auctionsDto.divineBarrier ?? false,
      hazardShield: auctionsDto.hazardShield ?? false,
      sniperShield: auctionsDto.sniperShield ?? false,
      dramonShield: auctionsDto.dramonShield ?? false,
      yinYangWand: auctionsDto.yinYangWand ?? false,
    };
  }
}
