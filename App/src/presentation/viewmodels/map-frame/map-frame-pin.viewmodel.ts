import type { CoordinatesViewModel } from "@/presentation/viewmodels/quest/coordinates.viewmodel";

export interface MapFramePinViewModel {
  coordinates: CoordinatesViewModel;
  label?: string | null;
}
