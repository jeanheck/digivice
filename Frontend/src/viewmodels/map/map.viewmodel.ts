import type { MapRegion } from "@/types/map-region.type";

export interface MapViewModel {
  locationRegion: MapRegion;
  locationImageUrl: string | null;
}
