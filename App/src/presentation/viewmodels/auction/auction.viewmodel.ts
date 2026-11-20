import type { AuctionStatus } from "@/shared/types/auction-status.type";

export interface AuctionViewModel {
  id: string;
  equipmentId: number;
  status: AuctionStatus;
  bid: number;
  resale: number;
}
