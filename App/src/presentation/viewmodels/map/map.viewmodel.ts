import type { MapRegion } from "@/shared/types/map-region.type";

export interface MapViewModel {
  locationRegion: MapRegion;
  locationImageUrl: string | null;
}
