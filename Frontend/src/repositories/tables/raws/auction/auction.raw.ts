import type { AuctionEntryRaw } from "./auction-entry.raw";

export type AuctionRaw = AuctionEntryRaw & {
  id: string;
};
