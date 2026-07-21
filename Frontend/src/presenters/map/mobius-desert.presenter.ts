import { ImageCatalog } from "@/catalogs/image.catalog";
import { DesertAreasMapRepository } from "@/repositories/desert-areas-map.repository";
import type { DesertAreaDetailsViewModel } from "@/viewmodels/desert/desert-area-details.viewmodel";
import type { DesertAreaMapCellViewModel } from "@/viewmodels/desert/desert-area-map-cell.viewmodel";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import type { DesertAreaTypeViewModel } from "@/viewmodels/desert/desert-area-type.viewmodel";

const IMAGE_NAME_BY_AREA_TYPE: Partial<Record<DesertAreaTypeViewModel, string>> = {
  noiseDesertS: "Noise Desert S",
  mirageTower: "Mirage Tower",
};

const IMAGE_NAME_BY_LOCATION_ID: Record<string, string> = {
  "0258": "Mobius Desert",
  "0259": "Mobius Desert 2",
};

export class MobiusDesertPresenter {
  public static getAreaDetails(area: DesertAreaViewModel | null): DesertAreaDetailsViewModel | null {
    if (area === null) {
      return null;
    }

    if (area.type === "noiseDesertS" || area.type === "mirageTower") {
      const imageName = IMAGE_NAME_BY_AREA_TYPE[area.type] ?? null;

      return {
        locationId: null,
        imageUrl: ImageCatalog.getMapImageUrl(imageName),
        coordinates: null,
      };
    }

    if (area.type !== "normal") {
      return null;
    }

    const areaRaw = DesertAreasMapRepository.findByLabel(area.label);

    if (areaRaw === null) {
      return null;
    }

    const imageName = IMAGE_NAME_BY_LOCATION_ID[areaRaw.locationId] ?? null;

    return {
      locationId: areaRaw.locationId,
      imageUrl: ImageCatalog.getMapImageUrl(imageName),
      coordinates: areaRaw.cell.coordinates ?? null,
    };
  }

  public static getCell(locationId: string, mapVariant: number): DesertAreaMapCellViewModel | null {
    if (mapVariant <= 0) {
      return null;
    }

    const cellRaw = DesertAreasMapRepository.getCell(locationId, String(mapVariant));

    if (cellRaw === null) {
      return null;
    }

    return cellRaw as DesertAreaMapCellViewModel;
  }
}
