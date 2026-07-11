export type SeabedDirectionDockType = "normal" | "dead-end";

export type DockLabelPosition = "above" | "below" | "left" | "right";

export interface SeabedDirectionDockRaw {
  location: string;
  x: number;
  y: number;
  type: SeabedDirectionDockType;
  labelPlacement: DockLabelPosition;
}
