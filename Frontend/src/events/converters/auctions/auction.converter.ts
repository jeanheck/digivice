import type { AuctionDTO } from "@/events/dto/auctions/auction.dto";
import type { Auction } from "@/models";

export class AuctionConverter {
    public static convert(auctionDto: AuctionDTO): Auction {
        return {
            id: auctionDto.id,
            hasParticipated: auctionDto.value !== undefined && auctionDto.value !== 0,
        };
    }
}
