export type SeabedRouteDockType = "normal" | "dead-end";

export interface SeabedRouteDockRaw {
  location: string;
  x: number;
  y: number;
  type: SeabedRouteDockType;
}
