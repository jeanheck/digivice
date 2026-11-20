import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { SeabedDirectionConverter } from "@/presentation/presenters/converter/seabed-direction.converter";
import { SeabedDirectionsRepository } from "@/presentation/repositories/seabed-direction.repository";
import type { SeabedDirectionViewModel } from "@/presentation/viewmodels/seabed-modal/seabed-direction.viewmodel";

export class SeabedDocksPresenter {
  public static getAsukaImageUrl(): string | null {
    return ImageCatalog.getLocationImageUrl("Asuka");
  }

  public static getRoutes(): SeabedDirectionViewModel[] {
    const seabedDirectionsTable = SeabedDirectionsRepository.getAll();

    return Object.entries(seabedDirectionsTable).map(([routeId, seabedDirectionRaw]) => {
      return SeabedDirectionConverter.convert(routeId, seabedDirectionRaw);
    });
  }
}
