import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";

export class AuctionCurrentConverter {
	public static convert(auctionRaw: AuctionRaw | null): AuctionCurrentViewModel {
		if (auctionRaw === null) {
			return {
				isActive: false,
				equipmentId: null,
				purchasePrice: null,
				resalePrice: null,
				closesWhenKey: null,
			};
		}

		return {
			isActive: true,
			equipmentId: Number(auctionRaw.equipmentId),
			purchasePrice: auctionRaw.price,
			resalePrice: auctionRaw.resale,
			closesWhenKey: `auction.closesWhen.${auctionRaw.id}`,
		};
	}
}