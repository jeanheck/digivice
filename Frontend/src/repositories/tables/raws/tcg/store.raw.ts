export interface StoreInventoryItemRaw {
  cardId: string;
  price: number;
}

export interface StorePhaseRaw {
  startWhenLastMainQuestStepDone: string;
  finishWhenLastMainQuestStepDone: string;
  inventory: StoreInventoryItemRaw[];
}

export interface StoreRaw {
  locationId: string;
  phases: StorePhaseRaw[];
}
