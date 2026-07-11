import type { SeabedRouteRaw } from "@/repositories/tables/raws/dock/seabed-route.raw";
import type { SeabedRouteViewModel } from "@/viewmodels/dock/seabed-route.viewmodel";

export class SeabedRouteConverter {
  public static convert(routeId: string, seabedRouteRaw: SeabedRouteRaw): SeabedRouteViewModel {
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
