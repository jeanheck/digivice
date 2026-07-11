export type SeabedRouteDockType = "normal" | "dead-end";

export type SeabedRouteDockLabelPlacement = "above" | "below" | "left" | "right";

export interface SeabedRouteDockRaw {
  location: string;
  x: number;
  y: number;
  type: SeabedRouteDockType;
  labelPlacement: SeabedRouteDockLabelPlacement;
}
