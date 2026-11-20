import type { CoordinatesRaw } from "@/presentation/repositories/tables/raws/quest/coordinates.raw";
import type { LabelPlacement } from "@/shared/types/label-placement.type";

export interface MapDuelIslandRaw {
  id: string;
  coordinates?: CoordinatesRaw;
  labelPlacement?: LabelPlacement;
}
