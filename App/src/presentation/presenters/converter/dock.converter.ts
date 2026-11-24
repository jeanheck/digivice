import type { DockPinRaw, DockRaw } from "@/presentation/repositories/tables/raws/seabed/dock.raw";
import type { SeabedModalPinViewModel } from "@/presentation/viewmodels/seabed-modal/seabed-modal-pin.viewmodel";
import type { SeabedModalViewModel } from "@/presentation/viewmodels/seabed-modal/seabed-modal.viewmodel";

export class SeabedModalConverter {
  public static convert(dockRaw: DockRaw, imageUrl: string | null): SeabedModalViewModel {
    return {
      imageUrl,
      pins: dockRaw.docks.map((dockPinRaw) => {
        return SeabedModalConverter.toPin(dockPinRaw);
      }),
    };
  }

  private static toPin(dockPinRaw: DockPinRaw): SeabedModalPinViewModel {
    const labelKey =
      dockPinRaw.labelLocationId === undefined
        ? "seabed.dock"
        : `map.${dockPinRaw.labelLocationId}`;

    return {
      coordinates: {
        x: dockPinRaw.coordinates.x,
        y: dockPinRaw.coordinates.y,
      },
      labelKey,
    };
  }
}
