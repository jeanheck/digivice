import type { CoordinatesRaw } from "@/presentation/repositories/tables/raws/quest/coordinates.raw";
import type { MomentRaw } from "@/presentation/repositories/tables/raws/quest/moment.raw";
import type { LabelPlacement } from "@/shared/types/label-placement.type";

export interface MapBossRaw {
  id: string;
  coordinates?: CoordinatesRaw;
  labelPlacement?: LabelPlacement;
  moment?: MomentRaw;
}
