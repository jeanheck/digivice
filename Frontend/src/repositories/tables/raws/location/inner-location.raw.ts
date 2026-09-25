import type { CoordinatesRaw } from "@/repositories/tables/raws/quest/coordinates.raw";

export interface InnerLocationRaw {
  location: string;
  coordinates: CoordinatesRaw;
}
