import type { AuctionStatusConstant } from "@/constants/auction-status.constant";

export interface AuctionListItemViewModel {
    id: string;
    equipmentId: number;
    status: AuctionStatusConstant;
}
