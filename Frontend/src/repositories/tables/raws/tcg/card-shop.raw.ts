import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";

export interface CardShopInventoryItemRaw {
  cardId: string;
  price: number;
}

export interface CardShopPhaseRaw {
  mainQuestStepDone: NpcMainQuestStepDoneRaw;
  inventory: CardShopInventoryItemRaw[];
}

export interface CardShopCatalogRaw {
  locationId: string;
  imageName?: string;
  phases: CardShopPhaseRaw[];
}
