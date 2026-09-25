import type { DeepRequired } from "@/events/dto/deep-required";
import type { AuctionsDTO } from "@/events/dto/auctions.dto";
import type { Auctions } from "@/models/auctions";

export class AuctionsConverter {
  public static convert(auctionsDto: DeepRequired<AuctionsDTO>): Auctions {
    return {
      divineBarrier: auctionsDto.divineBarrier,
      hazardShield: auctionsDto.hazardShield,
      sniperShield: auctionsDto.sniperShield,
      dramonShield: auctionsDto.dramonShield,
      yinYangWand: auctionsDto.yinYangWand,
    };
  }
}
