import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { EquipmentConverter } from "@/presentation/presenters/converter/equipment.converter";
import type { EquipmentRaw } from "@/presentation/repositories/tables/raws/equipment/equipment.raw";
import type { WikiEquipmentShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-equipment-shop-item.viewmodel";

export class WikiEquipmentShopItemConverter {
  public static convert(
    equipmentId: string,
    price: number,
    equipmentRaw: EquipmentRaw,
    equipableDigimonNames: string[],
  ): WikiEquipmentShopItemViewModel {
    const equipment = EquipmentConverter.convert(
      Number(equipmentId),
      equipableDigimonNames,
      equipmentRaw,
    );

    return {
      equipmentId,
      price,
      imageUrl: ImageCatalog.getEquipmentImageUrl(equipmentRaw.imageName),
      equipment,
      attributes: equipment.attributes.map((equipmentAttribute) => {
        return {
          ...equipmentAttribute,
          iconUrl: ImageCatalog.getStatImageUrl(equipmentAttribute.attribute),
        };
      }),
    };
  }
}
