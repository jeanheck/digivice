import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export type WikiLocationMapLabelPlacementViewModel = "above" | "below" | "left" | "right";

export interface WikiLocationMapMarkerViewModel {
  id: string;
  nameKey: string;
  imageUrl: string | null;
  coordinates: CoordinatesViewModel;
  labelPlacement: WikiLocationMapLabelPlacementViewModel;
}
