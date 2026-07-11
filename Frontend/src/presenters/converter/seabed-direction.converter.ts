import type { SeabedDirectionRaw } from "@/repositories/tables/raws/seabed/seabed-direction.raw";
import type { SeabedDirectionViewModel } from "@/viewmodels/dock/seabed-direction.viewmodel";

export class SeabedDirectionConverter {
  public static convert(routeId: string, seabedDirectionRaw: SeabedDirectionRaw): SeabedDirectionViewModel {
    return {
      id: routeId,
      docks: seabedDirectionRaw.docks.map((dockRaw) => {
        return {
          location: dockRaw.location,
          x: dockRaw.x,
          y: dockRaw.y,
          type: dockRaw.type,
          labelPlacement: dockRaw.labelPlacement,
        };
      }),
    };
  }
}
