export type BoosterDroppedByKind = "enemy" | "tamer" | "duelIsland";

export interface BoosterDroppedByRaw {
  kind: BoosterDroppedByKind;
  id: string;
  locationOnly?: string;
}

export interface BoosterRaw {
  cards: number[];
  droppedBy?: BoosterDroppedByRaw[];
}
