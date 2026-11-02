import { ConsumableItemConverter } from "@/presentation/presenters/converter/consumable-item.converter";
import type { ConsumableItemRaw } from "@/presentation/repositories/tables/raws/consumable-item/consumable-item.raw";
import type { WikiItemShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-item-shop-item.viewmodel";

export class WikiItemShopItemConverter {
  public static convert(
    consumableItemId: string,
    price: number,
    consumableItemRaw: ConsumableItemRaw,
  ): WikiItemShopItemViewModel {
    const consumableItem = ConsumableItemConverter.convert(Number(consumableItemId), consumableItemRaw);

    return {
      consumableItemId,
      price,
      imageUrl: consumableItem.imageUrl,
      nameKey: consumableItem.nameKey,
      noteKey: consumableItem.noteKey,
      additionalInformationKey: consumableItem.additionalInformationKey,
    };
  }
}
