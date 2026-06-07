import type { AuctionStatus } from "@/viewmodels/auction/auction-status";

export interface AuctionListItemViewModel {
    id: string;
    equipmentId: number;
    openStep: number;
    closeStep: number;
    status: AuctionStatus;
}
