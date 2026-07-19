import type { AuctionStatusConstant } from "@/constants/auction-status.constant";
import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionConverter {
	public static convert(auctionRaw: AuctionRaw, status: AuctionStatusConstant): AuctionListItemViewModel {
		return {
			id: auctionRaw.id,
			equipmentId: Number(auctionRaw.equipmentId),
			status,
		};
	}

	public static convertCurrent(auctionRaw: AuctionRaw | null): AuctionCurrentViewModel {
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
