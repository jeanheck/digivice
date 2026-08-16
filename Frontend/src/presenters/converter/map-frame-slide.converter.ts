import type { MapFrameSlideViewModel } from "@/viewmodels/map-frame/map-frame-slide.viewmodel";
import type { CoordinatesViewModel } from "@/viewmodels/quest/coordinates.viewmodel";

export class MapFrameSlideConverter {
  public static convert(
    imageUrl: string | null,
    coordinates: CoordinatesViewModel,
    label?: string | null,
  ): MapFrameSlideViewModel {
    return {
      imageUrl,
      pins: [
        {
          coordinates,
          label: label ?? undefined,
        },
      ],
    };
  }
}
