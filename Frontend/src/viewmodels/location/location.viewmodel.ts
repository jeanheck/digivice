import type { LocationRegion } from "@/types/location-region.type";

export interface LocationViewModel {
  id: string;
  image: string;
  enemies: string[];
  region: LocationRegion;
  dock: boolean;
}
