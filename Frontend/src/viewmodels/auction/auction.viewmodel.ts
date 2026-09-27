import type { AuctionStatus } from "@/types/auction-status.type";

export interface AuctionViewModel {
  id: string;
  equipmentId: number;
  status: AuctionStatus;
  bid: number;
  resale: number;
}
