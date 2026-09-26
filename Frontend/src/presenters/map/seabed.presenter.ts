import { SeabedRoutesRepository } from "@/repositories/seabed-routes.repository";
import type { SeabedRouteLocationViewModel } from "@/viewmodels/seabed/seabed-route-location.viewmodel";

export class SeabedPresenter {
  public static getRouteLocation(
    routeId: number | null,
    locationId: string,
    mapVariant: number | null,
  ): SeabedRouteLocationViewModel | null {
    if (routeId === null || mapVariant === null) {
      return null;
    }

    const locationKey = `${locationId}-${mapVariant}`;
    const seabedRouteLocationRaw = SeabedRoutesRepository.getByRouteAndLocation(
      String(routeId),
      locationKey,
    );

    if (seabedRouteLocationRaw === null) {
      return null;
    }

    return seabedRouteLocationRaw as SeabedRouteLocationViewModel;
  }
}
