import type { SeabedDirectionDockType } from "@/repositories/tables/raws/seabed/seabed-direction-dock.raw";
import type { LabelPlacement } from "@/types/label-placement.type";

export interface SeabedDirectionDockViewModel {
  location: string;
  x: number;
  y: number;
  type: SeabedDirectionDockType;
  labelPlacement: LabelPlacement;
}
