import { SeabedRouteConverter } from "@/presenters/converter/seabed-route.converter";
import { SeabedRouteRepository } from "@/repositories/seabed-route.repository";
import type { SeabedRouteViewModel } from "@/viewmodels/dock/seabed-route.viewmodel";

export class DocksPresenter {
  public static getRoutes(): SeabedRouteViewModel[] {
    const seabedRoutesTable = SeabedRouteRepository.getAll();

    return Object.entries(seabedRoutesTable).map(([routeId, seabedRouteRaw]) => {
      return SeabedRouteConverter.convert(routeId, seabedRouteRaw);
    });
  }
}
