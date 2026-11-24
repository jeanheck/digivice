import type { CoordinatesRaw } from "@/presentation/repositories/tables/raws/quest/coordinates.raw";

export interface DockPinRaw {
  coordinates: CoordinatesRaw;
  labelLocationId?: string;
}

export interface DockRaw {
  docks: DockPinRaw[];
}
