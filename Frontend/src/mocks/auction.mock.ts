import type { AuctionStatus } from "@/viewmodels/auction/auction-status";

export interface AuctionMockEntry {
    id: string;
    equipmentId: number;
    openStep: number;
    closeStep: number;
    status: AuctionStatus;
    purchasePrice: number;
    resalePrice: number;
    closesWhenKey: string;
}

export const MOCK_WINDOW_OPEN_EQUIPMENT_ID: number | null = 1032;

export const AUCTION_MOCK_ENTRIES: AuctionMockEntry[] = [
    {
        id: "auction-divine-barrier",
        equipmentId: 281,
        openStep: 4,
        closeStep: 6,
        status: "participated",
        purchasePrice: 500,
        resalePrice: 7500,
        closesWhenKey: "auction.closesWhen.divineBarrier",
    },
    {
        id: "auction-hazard-shield",
        equipmentId: 1032,
        openStep: 15,
        closeStep: 21,
        status: "availableNow",
        purchasePrice: 800,
        resalePrice: 11000,
        closesWhenKey: "auction.closesWhen.hazardShield",
    },
    {
        id: "auction-sniper-shield",
        equipmentId: 1083,
        openStep: 28,
        closeStep: 34,
        status: "notYetOccurred",
        purchasePrice: 1200,
        resalePrice: 14000,
        closesWhenKey: "auction.closesWhen.sniperShield",
    },
    {
        id: "auction-dramon-shield",
        equipmentId: 1017,
        openStep: 36,
        closeStep: 43,
        status: "missed",
        purchasePrice: 1500,
        resalePrice: 16000,
        closesWhenKey: "auction.closesWhen.dramonShield",
    },
];
