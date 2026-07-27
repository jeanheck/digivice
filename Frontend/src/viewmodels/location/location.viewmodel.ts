import type { LocationRegionConstant } from "@/constants/location-region.constant";

export interface LocationViewModel {
  id: string;
  image: string;
  enemies: string[];
  region: LocationRegionConstant;
  dock: boolean;
}
