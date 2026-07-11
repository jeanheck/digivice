import { SeabedDirectionConverter } from "@/presenters/converter/seabed-direction.converter";
import { SeabedDirectionsRepository } from "@/repositories/seabed-direction.repository";
import type { SeabedDirectionViewModel } from "@/viewmodels/dock/seabed-direction.viewmodel";

export class DocksPresenter {
  public static getRoutes(): SeabedDirectionViewModel[] {
    const seabedRoutesTable = SeabedDirectionsRepository.getAll();

    return Object.entries(seabedRoutesTable).map(([routeId, seabedRouteRaw]) => {
      return SeabedDirectionConverter.convert(routeId, seabedRouteRaw);
    });
  }
}
