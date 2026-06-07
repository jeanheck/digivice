import type { AuctionRuntimeStatus } from "@/viewmodels/auction/auction-status";

export interface AuctionListItemViewModel {
    id: string;
    equipmentId: number;
    openStep: number;
    closeStep: number;
    status: AuctionRuntimeStatus;
}
