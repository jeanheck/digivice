import type { MapFrameSlideViewModel } from "@/presentation/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { WikiLocationEncounterLineViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-location-encounter-line.viewmodel";
import type { WikiLocationMapMarkerViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-location-map-marker.viewmodel";

export interface WikiLocationsPanelViewModel {
  asukaSlides: MapFrameSlideViewModel[];
  localSlides: MapFrameSlideViewModel[];
  selectedLocationLabelKey: string | null;
  encounterLines: WikiLocationEncounterLineViewModel[];
  mapMarkers: WikiLocationMapMarkerViewModel[];
}
