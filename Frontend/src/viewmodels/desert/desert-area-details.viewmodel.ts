import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export interface DesertAreaDetailsViewModel {
  locationId: string;
  coordinates: CoordinatesViewModel | null;
}
