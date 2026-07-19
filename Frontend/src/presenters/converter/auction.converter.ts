import type { AuctionStatusConstant } from "@/constants/auction-status.constant";
import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";
import type { AuctionViewModel } from "@/viewmodels/auction/auction.viewmodel";

export class AuctionConverter {
	public static convert(auctionRaw: AuctionRaw, status: AuctionStatusConstant): AuctionViewModel {
		return {
			id: auctionRaw.id,
			equipmentId: Number(auctionRaw.equipmentId),
			status,
		};
	}
}
