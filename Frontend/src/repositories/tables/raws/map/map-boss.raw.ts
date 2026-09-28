import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import type { LabelPlacement } from "@/types/label-placement.type";

export interface MapBossRaw {
  id: string;
  coordinates?: CoordinatesRaw;
  labelPlacement?: LabelPlacement;
}
