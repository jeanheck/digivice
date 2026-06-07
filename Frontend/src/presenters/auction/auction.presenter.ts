import { AUCTION_MOCK_ENTRIES, MOCK_WINDOW_OPEN_EQUIPMENT_ID } from "@/mocks/auction.mock";
import type { AuctionCardViewModel } from "@/viewmodels/auction/auction-card.viewmodel";
import type { AuctionCurrentViewModel } from "@/viewmodels/auction/auction-current.viewmodel";
import type { AuctionListItemViewModel } from "@/viewmodels/auction/auction-list-item.viewmodel";

export class AuctionPresenter {
    public static getAuctionCardViewModel(): AuctionCardViewModel {
        const activeAuction = this.getActiveAuction();

        return {
            isActive: activeAuction !== null,
            activeEquipmentId: activeAuction?.equipmentId ?? null,
        };
    }

    public static getAuctionCurrentViewModel(): AuctionCurrentViewModel {
        const activeAuction = this.getActiveAuction();

        if (activeAuction === null) {
            return {
                isActive: false,
                equipmentId: null,
                purchasePrice: null,
                resalePrice: null,
                closesWhenKey: null,
            };
        }

        const activeAuctionMockEntry = AUCTION_MOCK_ENTRIES.find((auctionMockEntry) => {
            return auctionMockEntry.id === activeAuction.id;
        });

        if (activeAuctionMockEntry === undefined) {
            return {
                isActive: true,
                equipmentId: activeAuction.equipmentId,
                purchasePrice: null,
                resalePrice: null,
                closesWhenKey: null,
            };
        }

        return {
            isActive: true,
            equipmentId: activeAuctionMockEntry.equipmentId,
            purchasePrice: activeAuctionMockEntry.purchasePrice,
            resalePrice: activeAuctionMockEntry.resalePrice,
            closesWhenKey: activeAuctionMockEntry.closesWhenKey,
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

    public static getAuctionHistoryViewModels(): AuctionListItemViewModel[] {
        return this.getAuctionListViewModels().filter((auctionListItemViewModel) => {
            return auctionListItemViewModel.status !== "availableNow";
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
