import type { MainQuestAvailabilityWindowRaw } from "@/presentation/repositories/tables/raws/quest/main-quest-availability-window.raw";

export interface CardShopPhaseRaw {
  mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw;
  inventory: string[];
}

export interface CardShopCatalogRaw {
  locationId: string;
  imageName?: string;
  phases: CardShopPhaseRaw[];
}

export interface CardShopCardRaw {
  cardShopId: string;
  mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw;
}
