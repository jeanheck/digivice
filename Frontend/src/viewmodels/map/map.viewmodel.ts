import type { LocationRegion } from "@/types/location-region.type";

export interface MapViewModel {
  locationRegion: LocationRegion;
  locationImageUrl: string | null;
}
