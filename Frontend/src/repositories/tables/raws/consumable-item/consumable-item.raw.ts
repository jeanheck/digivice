export interface ConsumableItemDroppedByRaw {
  kind: "enemy";
  id: string;
  locationOnly?: string;
}

export interface ConsumableItemRaw {
  resaleValue: number;
  soldInStore: boolean;
  droppedBy: ConsumableItemDroppedByRaw[];
}
