import type { AuctionRaw } from "@/repositories/tables/raws/auction/auction.raw";
import type { AuctionCardViewModel } from "@/viewmodels/auction/auction-card.viewmodel";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";
import type { AuctionStatus } from "@/viewmodels/auction/auction-status";

export class AuctionConverter {
    public static convertListItem(auctionRaw: AuctionRaw, status: AuctionStatus): AuctionListItemViewModel {
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

    public static convertCard(isActive: boolean, activeEquipmentId: number | null): AuctionCardViewModel {
        return {
            isActive,
            activeEquipmentId,
        };
    }
}
