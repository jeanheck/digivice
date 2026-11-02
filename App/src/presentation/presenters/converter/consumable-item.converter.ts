import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import type { ConsumableItemRaw } from "@/presentation/repositories/tables/raws/consumable-item/consumable-item.raw";
import type { ConsumableItemViewModel } from "@/presentation/viewmodels/consumable-item/consumable-item.viewmodel";

const DefaultConsumableItemImageName = "Pill";

export class ConsumableItemConverter {
  public static convert(
    consumableItemId: number,
    consumableItemRaw: ConsumableItemRaw,
  ): ConsumableItemViewModel {
    return {
      id: consumableItemId,
      nameKey: `consumableItems.${consumableItemId}.name`,
      resaleValue: consumableItemRaw.resaleValue,
      imageUrl: ImageCatalog.getConsumableItemImageUrl(
        consumableItemRaw.imageName ?? DefaultConsumableItemImageName,
      ),
      noteKey: `consumableItems.${consumableItemId}.note`,
      additionalInformationKey: `consumableItems.${consumableItemId}.additionalInformation`,
    };
  }
}
