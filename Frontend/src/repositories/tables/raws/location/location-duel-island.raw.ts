import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";
import type { LocationMapLabelPlacementRaw } from "@/repositories/tables/raws/location/location-map-label-placement.raw";

export interface LocationDuelIslandRaw {
  id: string;
  coordinates?: CoordinatesRaw;
  labelPlacement?: LocationMapLabelPlacementRaw;
}
