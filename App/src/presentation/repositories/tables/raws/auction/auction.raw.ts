import type { MainQuestAvailabilityWindowRaw } from "@/presentation/repositories/tables/raws/quest/main-quest-availability-window.raw";

export interface AuctionRaw {
  id: string;
  equipmentId: string;
  mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw;
  bid: number;
  resale: number;
}
