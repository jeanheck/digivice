import type { AuctionModalStatus } from "@/viewmodels/auction/auction-status";

export interface AuctionMockEntry {
    id: string;
    equipmentId: number;
    openStep: number;
    closeStep: number;
    status: AuctionModalStatus;
}

export const MOCK_WINDOW_OPEN_EQUIPMENT_ID: number | null = 1032;

export const AUCTION_MOCK_ENTRIES: AuctionMockEntry[] = [
    {
        id: "auction-divine-barrier",
        equipmentId: 281,
        openStep: 4,
        closeStep: 6,
        status: "bought",
    },
    {
        id: "auction-hazard-shield",
        equipmentId: 1032,
        openStep: 15,
        closeStep: 21,
        status: "participatedWithoutPurchase",
    },
    {
        id: "auction-sniper-shield",
        equipmentId: 1083,
        openStep: 28,
        closeStep: 34,
        status: "notYetOccurred",
    },
    {
        id: "auction-dramon-shield",
        equipmentId: 1017,
        openStep: 36,
        closeStep: 43,
        status: "missed",
    },
];
