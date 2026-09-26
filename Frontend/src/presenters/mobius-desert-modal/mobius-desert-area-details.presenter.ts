import { ImageCatalog } from "@/catalogs/image.catalog";
import { MobiusDesertAreasMapRepository } from "@/repositories/mobius-desert-areas-map.repository";
import type { DesertAreaDetailsViewModel } from "@/viewmodels/desert/desert-area-details.viewmodel";
import type { DesertAreaViewModel } from "@/viewmodels/desert/desert-area.viewmodel";
import type { MobiusDesertAreaType } from "@/types/mobius-desert-area-type.type";

const ImageNameByAreaType: Partial<Record<MobiusDesertAreaType, string>> = {
  noiseDesertS: "Noise Desert S",
  mirageTower: "Mirage Tower",
};

const ImageNameByLocationId: Record<string, string> = {
  "0258": "Mobius Desert",
  "0259": "Mobius Desert 2",
};

export class MobiusDesertAreaDetailsPresenter {
  public static getAreaDetails(
    area: DesertAreaViewModel | null,
  ): DesertAreaDetailsViewModel | null {
    if (area === null) {
      return null;
    }

    if (area.type === "noiseDesertS" || area.type === "mirageTower") {
      const imageName = ImageNameByAreaType[area.type] ?? null;

      return {
        locationId: null,
        imageUrl: ImageCatalog.getLocationImageUrl(imageName),
        coordinates: null,
      };
    }

    if (area.type !== "normal") {
      return null;
    }

    const areaRaw = MobiusDesertAreasMapRepository.findByLabel(area.label);

    if (areaRaw === null) {
      return null;
    }

    const imageName = ImageNameByLocationId[areaRaw.locationId] ?? null;

    return {
      locationId: areaRaw.locationId,
      imageUrl: ImageCatalog.getLocationImageUrl(imageName),
      coordinates: areaRaw.cell.coordinates ?? null,
    };
  }
}
