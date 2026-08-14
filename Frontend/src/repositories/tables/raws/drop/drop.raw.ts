export type DropType = "equipment" | "consumableItem" | "booster";

export interface DropRaw {
  id: number;
  type: DropType;
}
