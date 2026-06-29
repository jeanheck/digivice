import type { AuctionStepsRaw } from "./auction-steps.raw";

export interface AuctionRaw {
    id: string;
    equipmentId: string;
    steps: AuctionStepsRaw;
    price: number;
    resale: number;
}
