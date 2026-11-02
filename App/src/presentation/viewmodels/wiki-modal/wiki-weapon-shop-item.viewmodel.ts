import type { EquipmentViewModel } from "@/presentation/viewmodels/digimon/equipment.viewmodel";
import type { WikiWeaponShopItemAttributeViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-weapon-shop-item-attribute.viewmodel";

export interface WikiWeaponShopItemViewModel {
  equipmentId: string;
  price: number;
  equipment: EquipmentViewModel;
  attributes: WikiWeaponShopItemAttributeViewModel[];
}
