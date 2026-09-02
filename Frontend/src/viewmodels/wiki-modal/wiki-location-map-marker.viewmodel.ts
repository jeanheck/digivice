import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export type WikiLocationMapLabelPlacementViewModel = "above" | "below" | "left" | "right";

export type WikiLocationMapMarkerKindViewModel = "npc" | "boss";

export interface WikiLocationMapMarkerViewModel {
  id: string;
  kind: WikiLocationMapMarkerKindViewModel;
  nameKey?: string;
  name?: string;
  imageUrl: string | null;
  coordinates: CoordinatesViewModel;
  labelPlacement: WikiLocationMapLabelPlacementViewModel;
}
