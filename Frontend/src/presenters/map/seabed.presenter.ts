import { SeabedRoutesRepository } from "@/repositories/seabed-routes.repository";
import type { SeabedRouteLocationViewModel } from "@/viewmodels/seabed/seabed-route-location.viewmodel";

export class SeabedPresenter {
  public static getRouteLocation(routeId: number, locationId: string): SeabedRouteLocationViewModel | null {
    const seabedRouteLocationRaw = SeabedRoutesRepository.getByRouteAndLocation(String(routeId), locationId);

    if (seabedRouteLocationRaw === null) {
      return null;
    }

    return seabedRouteLocationRaw as SeabedRouteLocationViewModel;
  }
}
