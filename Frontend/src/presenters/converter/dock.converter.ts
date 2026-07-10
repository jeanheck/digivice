import type { DockRaw } from "@/repositories/tables/raws/dock/dock.raw";
import type { DockViewModel } from "@/viewmodels/dock/dock.viewmodel";

export class DockConverter {
  public static convert(dockRaw: DockRaw, imageUrl: string | null): DockViewModel {
    return {
      imageUrl,
      coordinates: {
        x: dockRaw.coordinates.x,
        y: dockRaw.coordinates.y,
      },
    };
  }
}
