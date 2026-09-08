export type BoosterDroppedByKind = "tamer" | "duelIsland";

export interface BoosterDroppedByRaw {
  kind: BoosterDroppedByKind;
  id: string;
}

export interface BoosterRaw {
  cards: number[];
  droppedBy: BoosterDroppedByRaw[];
}
