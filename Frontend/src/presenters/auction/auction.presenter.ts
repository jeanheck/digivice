import { AUCTION_MOCK_ENTRIES, MOCK_WINDOW_OPEN_EQUIPMENT_ID } from "@/mocks/auction.mock";
import type { AuctionCardViewModel } from "@/viewmodels/auction/auction-card.viewmodel";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionPresenter {
    public static getAuctionCardViewModel(): AuctionCardViewModel {
        const activeAuction = this.getActiveAuction();

        return {
            isActive: activeAuction !== null,
            activeEquipmentId: activeAuction?.equipmentId ?? null,
        };
    }

    public static getAuctionListViewModels(): AuctionListItemViewModel[] {
        return AUCTION_MOCK_ENTRIES.map((auctionMockEntry) => {
            return {
                id: auctionMockEntry.id,
                equipmentId: auctionMockEntry.equipmentId,
                openStep: auctionMockEntry.openStep,
                closeStep: auctionMockEntry.closeStep,
                status: auctionMockEntry.status,
            };
        });
    }

    public static getActiveAuction(): AuctionListItemViewModel | null {
        if (MOCK_WINDOW_OPEN_EQUIPMENT_ID === null) {
            return null;
        }

        const activeAuctionMockEntry = AUCTION_MOCK_ENTRIES.find((auctionMockEntry) => {
            return auctionMockEntry.equipmentId === MOCK_WINDOW_OPEN_EQUIPMENT_ID;
        });

        if (activeAuctionMockEntry === undefined) {
            return null;
        }

        return {
            id: activeAuctionMockEntry.id,
            equipmentId: activeAuctionMockEntry.equipmentId,
            openStep: activeAuctionMockEntry.openStep,
            closeStep: activeAuctionMockEntry.closeStep,
            status: "availableNow",
        };
    }
}
