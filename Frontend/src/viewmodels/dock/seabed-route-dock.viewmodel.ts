import type {
  DockLabelPosition,
  SeabedDirectionDockType,
} from "@/repositories/tables/raws/seabed/seabed-direction-dock.raw";

export interface SeabedRouteDockViewModel {
  location: string;
  x: number;
  y: number;
  type: SeabedDirectionDockType;
  labelPlacement: DockLabelPosition;
}
