import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";

export interface StoreInventoryItemRaw {
  cardId: string;
  price: number;
}

export interface StorePhaseRaw {
  mainQuestStepDone: NpcMainQuestStepDoneRaw;
  inventory: StoreInventoryItemRaw[];
}

export interface StoreRaw {
  locationId: string;
  imageName?: string;
  phases: StorePhaseRaw[];
}
