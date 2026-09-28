import type { MainQuestAvailabilityWindowRaw } from "@/repositories/tables/raws/quest/main-quest-availability-window.raw";

export interface AuctionRaw {
  id: string;
  equipmentId: string;
  mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw;
  bid: number;
  resale: number;
}
