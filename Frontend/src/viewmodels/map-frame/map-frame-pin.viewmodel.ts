import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export interface MapFramePinViewModel {
  coordinates: CoordinatesViewModel;
  label?: string | null;
}
