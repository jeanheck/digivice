import type { SeabedDirectionRaw } from "@/repositories/tables/raws/seabed/seabed-direction.raw";
import type { SeabedRouteViewModel } from "@/viewmodels/dock/seabed-route.viewmodel";

export class SeabedRouteConverter {
  public static convert(routeId: string, seabedRouteRaw: SeabedDirectionRaw): SeabedRouteViewModel {
    return {
      id: routeId,
      docks: seabedRouteRaw.docks.map((dockRaw) => {
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
