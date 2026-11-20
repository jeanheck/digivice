import type { CoordinatesRaw } from "@/presentation/repositories/tables/raws/quest/coordinates.raw";

export interface InnerLocationRaw {
  location: string;
  coordinates: CoordinatesRaw;
}
