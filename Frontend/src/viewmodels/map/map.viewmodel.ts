import type { LocationRegionConstant } from "@/constants/location-region.constant";

export interface MapViewModel {
  locationRegion: LocationRegionConstant;
  locationImageUrl: string | null;
}
