import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export interface ZoomedLocationMapItem {
  imageUrl: string | null;
  coordinates: CoordinatesViewModel;
  labelKey: string;
}
