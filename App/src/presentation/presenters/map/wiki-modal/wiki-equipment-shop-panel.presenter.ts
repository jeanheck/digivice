import { WikiEquipmentShopItemConverter } from "@/presentation/presenters/converter/wiki-equipment-shop-item.converter";
import { DigimonRepository } from "@/presentation/repositories/digimon.repository";
import { EquipmentRepository } from "@/presentation/repositories/equipment.repository";
import { EquipmentShopRepository } from "@/presentation/repositories/equipment-shop.repository";
import type { WikiEquipmentShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-equipment-shop-item.viewmodel";
import type { WikiEquipmentShopViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-equipment-shop.viewmodel";

export class WikiEquipmentShopPanelPresenter {
  public static getViewModel(
    equipmentShopId: string,
    translateEquipmentName: (equipmentId: string) => string,
  ): WikiEquipmentShopViewModel {
    const equipmentShopRaw = EquipmentShopRepository.getById(equipmentShopId);
    if (equipmentShopRaw === undefined) {
      return {
        items: [],
        locationId: null,
      };
    }

    const items = equipmentShopRaw.inventory
      .map((equipmentId) => {
        return this.getItem(equipmentId);
      })
      .filter((item): item is WikiEquipmentShopItemViewModel => {
        return item !== null;
      });

    if (equipmentShopRaw.inventoryOrder === "alphabetical") {
      items.sort((firstItem, secondItem) => {
        return translateEquipmentName(firstItem.equipmentId).localeCompare(
          translateEquipmentName(secondItem.equipmentId),
        );
      });
    }

    return {
      items,
      locationId: equipmentShopRaw.locationId,
    };
  }

  private static getItem(equipmentId: string): WikiEquipmentShopItemViewModel | null {
    const equipmentRaw = EquipmentRepository.getEquipmentById(Number(equipmentId));
    if (equipmentRaw.price === undefined) {
      return null;
    }

    const equipableDigimonNames = equipmentRaw.equipableDigimon.map((digimonId) => {
      return DigimonRepository.getNameById(Number(digimonId));
    });

    return WikiEquipmentShopItemConverter.convert(
      equipmentId,
      equipmentRaw.price,
      equipmentRaw,
      equipableDigimonNames,
    );
  }
}
