import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export interface DockViewModel {
  imageUrl: string | null;
  coordinates: CoordinatesViewModel;
}
