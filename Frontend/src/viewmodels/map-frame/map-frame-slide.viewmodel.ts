import type { MapFramePinViewModel } from "./map-frame-pin.viewmodel";

export interface MapFrameSlideViewModel {
  imageUrl: string | null;
  pins: MapFramePinViewModel[];
}
