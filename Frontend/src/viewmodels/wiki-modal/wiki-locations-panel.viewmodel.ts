import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { WikiLocationEncounterLineViewModel } from "@/viewmodels/wiki-modal/wiki-location-encounter-line.viewmodel";

export interface WikiLocationsPanelViewModel {
  asukaSlides: MapFrameSlideViewModel[];
  localSlides: MapFrameSlideViewModel[];
  selectedLocationLabelKey: string | null;
  encounterLines: WikiLocationEncounterLineViewModel[];
}
