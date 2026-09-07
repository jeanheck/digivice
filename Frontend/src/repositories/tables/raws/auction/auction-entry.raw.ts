import type { AuctionStepsRaw } from "./auction-steps.raw";

export interface AuctionEntryRaw {
  equipmentId: string;
  steps: AuctionStepsRaw;
  price: number;
  resale: number;
}
