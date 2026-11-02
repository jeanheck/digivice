import { WikiItemShopItemConverter } from "@/presentation/presenters/converter/wiki-item-shop-item.converter";
import { ConsumableItemRepository } from "@/presentation/repositories/consumable-item.repository";
import { ItemShopRepository } from "@/presentation/repositories/item-shop.repository";
import type { ItemShopInventoryItemRaw } from "@/presentation/repositories/tables/raws/shop/item-shop.raw";
import type { WikiItemShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-item-shop-item.viewmodel";
import type { WikiItemShopViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-item-shop.viewmodel";

export class WikiItemShopPanelPresenter {
  public static getViewModel(
    itemShopId: string,
    translateConsumableItemName: (consumableItemId: string) => string,
  ): WikiItemShopViewModel {
    const itemShopRaw = ItemShopRepository.getById(itemShopId);
    if (itemShopRaw === undefined) {
      return {
        items: [],
        locationId: null,
      };
    }

    const items = itemShopRaw.inventory
      .map((inventoryItem) => {
        return this.getItem(inventoryItem);
      })
      .filter((item): item is WikiItemShopItemViewModel => {
        return item !== null;
      });

    if (itemShopRaw.inventoryOrder === "alphabetical") {
      items.sort((firstItem, secondItem) => {
        return translateConsumableItemName(firstItem.consumableItemId).localeCompare(
          translateConsumableItemName(secondItem.consumableItemId),
        );
      });
    }

    return {
      items,
      locationId: itemShopRaw.locationId,
    };
  }

  private static getItem(inventoryItem: ItemShopInventoryItemRaw): WikiItemShopItemViewModel | null {
    const consumableItemRaw = ConsumableItemRepository.getById(Number(inventoryItem.consumableItemId));
    if (consumableItemRaw === undefined) {
      return null;
    }

    return WikiItemShopItemConverter.convert(
      inventoryItem.consumableItemId,
      inventoryItem.price,
      consumableItemRaw,
    );
  }
}
