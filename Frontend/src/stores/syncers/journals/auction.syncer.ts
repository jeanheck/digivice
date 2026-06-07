import type { AuctionDTO } from "@/events/dto/auctions/auction.dto";
import type { Auction } from "@/models";

export class AuctionSyncer {
    public static sync(previousAuction: Auction, newAuctionDto: AuctionDTO): void {
        if (newAuctionDto.value !== undefined) {
            previousAuction.hasParticipated = newAuctionDto.value !== 0;
        }
    }
}
