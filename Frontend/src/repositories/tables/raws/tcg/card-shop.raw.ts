import type { MainQuestAvailabilityWindowRaw } from "@/repositories/tables/raws/quest/main-quest-availability-window.raw";

export interface CardShopInventoryItemRaw {
  cardId: string;
  price: number;
}

export interface CardShopPhaseRaw {
  mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw;
  inventory: CardShopInventoryItemRaw[];
}

export interface CardShopCatalogRaw {
  locationId: string;
  imageName?: string;
  phases: CardShopPhaseRaw[];
}

export interface CardShopCardRaw {
  cardShopId: string;
  mainQuestAvailabilityWindow: MainQuestAvailabilityWindowRaw;
  price: number;
}
