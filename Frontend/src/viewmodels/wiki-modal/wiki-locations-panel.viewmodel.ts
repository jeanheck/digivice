import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";

export interface WikiLocationsPanelViewModel {
  asukaSlides: MapFrameSlideViewModel[];
  localSlides: MapFrameSlideViewModel[];
  selectedLocationLabelKey: string | null;
}
