import type { MainQuestAvailabilityWindowRaw } from "./main-quest-availability-window.raw";

export interface AuctionRaw {
  id: string;
  equipmentId: string;
  mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw;
  bid: number;
  resale: number;
}
