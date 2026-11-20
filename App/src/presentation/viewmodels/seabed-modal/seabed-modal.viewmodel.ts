import type { CoordinatesViewModel } from "@/presentation/viewmodels/quest/coordinates.viewmodel";

export interface SeabedModalViewModel {
  imageUrl: string | null;
  coordinates: CoordinatesViewModel;
}
