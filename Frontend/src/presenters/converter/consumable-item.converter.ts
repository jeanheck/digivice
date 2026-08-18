import type { ConsumableItemRaw } from "@/repositories/tables/raws/consumable-item/consumable-item.raw";
import type { ConsumableItemViewModel } from "@/viewmodels/consumable-item/consumable-item.viewmodel";

export class ConsumableItemConverter {
  public static convert(
    consumableItemId: number,
    consumableItemRaw: ConsumableItemRaw,
  ): ConsumableItemViewModel {
    return {
      id: consumableItemId,
      nameKey: `consumableItems.${consumableItemId}.name`,
      resaleValue: consumableItemRaw.resaleValue,
      soldInStore: consumableItemRaw.soldInStore,
      noteKey: `consumableItems.${consumableItemId}.note`,
      additionalInformationKey: `consumableItems.${consumableItemId}.additionalInformation`,
    };
  }
}
