import type { AuctionStatusConstant } from "@/constants/auction-status.constant";

export interface AuctionViewModel {
    id: string;
    equipmentId: number;
    status: AuctionStatusConstant;
}
