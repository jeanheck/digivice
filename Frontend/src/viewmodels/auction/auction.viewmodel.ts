import type { AuctionStatus } from "@/types/auction-status.type";

export interface AuctionViewModel {
  id: string;
  equipmentId: number;
  status: AuctionStatus;
  price: number;
  resale: number;
}
