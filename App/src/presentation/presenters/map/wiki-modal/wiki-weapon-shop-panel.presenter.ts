import { EquipmentConverter } from "@/presentation/presenters/converter/equipment.converter";
import { DigimonRepository } from "@/presentation/repositories/digimon.repository";
import { EquipmentRepository } from "@/presentation/repositories/equipment.repository";
import { WeaponShopRepository } from "@/presentation/repositories/weapon-shop.repository";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import type { WeaponShopInventoryItemRaw } from "@/presentation/repositories/tables/raws/shop/weapon-shop.raw";
import type { WikiWeaponShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-weapon-shop-item.viewmodel";
import type { WikiWeaponShopViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-weapon-shop.viewmodel";

export class WikiWeaponShopPanelPresenter {
  public static getViewModel(weaponShopId: string): WikiWeaponShopViewModel {
    const weaponShopRaw = WeaponShopRepository.getById(weaponShopId);
    if (weaponShopRaw === undefined) {
      return {
        items: [],
        locationId: null,
      };
    }

    return {
      items: weaponShopRaw.inventory.map((inventoryItem) => {
        return this.getItem(inventoryItem);
      }),
      locationId: weaponShopRaw.locationId,
    };
  }

  private static getItem(inventoryItem: WeaponShopInventoryItemRaw): WikiWeaponShopItemViewModel {
    const equipmentId = Number(inventoryItem.equipmentId);
    const equipmentRaw = EquipmentRepository.getEquipmentById(equipmentId);
    const equipableDigimonNames = equipmentRaw.equipableDigimon.map((digimonId) => {
      return DigimonRepository.getNameById(Number(digimonId));
    });

    const equipment = EquipmentConverter.convert(equipmentId, equipableDigimonNames, equipmentRaw);

    return {
      equipmentId: inventoryItem.equipmentId,
      price: inventoryItem.price,
      imageUrl: ImageCatalog.getWeaponImageUrl(equipmentRaw.imageName),
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
