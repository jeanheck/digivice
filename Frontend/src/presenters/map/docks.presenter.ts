import { SeabedDirectionConverter } from "@/presenters/converter/seabed-direction.converter";
import { SeabedDirectionsRepository } from "@/repositories/seabed-direction.repository";
import type { SeabedDirectionViewModel } from "@/viewmodels/dock/seabed-direction.viewmodel";

export class DocksPresenter {
  public static getRoutes(): SeabedDirectionViewModel[] {
    const seabedDirectionsTable = SeabedDirectionsRepository.getAll();

    return Object.entries(seabedDirectionsTable).map(([routeId, seabedDirectionRaw]) => {
      return SeabedDirectionConverter.convert(routeId, seabedDirectionRaw);
    });
  }
}
