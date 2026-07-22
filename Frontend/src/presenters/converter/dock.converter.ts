import type { DockRaw } from "@/repositories/tables/raws/seabed/dock.raw";
import type { SeabedModalViewModel } from "@/viewmodels/seabed-modal/seabed-modal.viewmodel";

export class SeabedModalConverter {
  public static convert(dockRaw: DockRaw, imageUrl: string | null): SeabedModalViewModel {
    return {
      imageUrl,
      coordinates: {
        x: dockRaw.coordinates.x,
        y: dockRaw.coordinates.y,
      },
    };
  }
}
