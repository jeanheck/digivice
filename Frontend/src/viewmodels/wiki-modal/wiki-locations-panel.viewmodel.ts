import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { WikiLocationEncounterLineViewModel } from "@/viewmodels/wiki-modal/wiki-location-encounter-line.viewmodel";
import type { WikiLocationMapMarkerViewModel } from "@/viewmodels/wiki-modal/wiki-location-map-marker.viewmodel";

export interface WikiLocationsPanelViewModel {
  asukaSlides: MapFrameSlideViewModel[];
  localSlides: MapFrameSlideViewModel[];
  selectedLocationLabelKey: string | null;
  encounterLines: WikiLocationEncounterLineViewModel[];
  mapMarkers: WikiLocationMapMarkerViewModel[];
}
