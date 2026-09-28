import type { MapRegion } from "@/types/map-region.type";

export interface LocationViewModel {
  id: string;
  image: string;
  enemies: string[];
  region: MapRegion;
  dock: boolean;
}
