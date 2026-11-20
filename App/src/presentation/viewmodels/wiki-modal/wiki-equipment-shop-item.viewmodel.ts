import type { EquipmentViewModel } from "@/presentation/viewmodels/digimon/equipment.viewmodel";
import type { WikiEquipmentShopItemAttributeViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-equipment-shop-item-attribute.viewmodel";

export interface WikiEquipmentShopItemViewModel {
  equipmentId: string;
  price: number;
  imageUrl: string | null;
  equipment: EquipmentViewModel;
  attributes: WikiEquipmentShopItemAttributeViewModel[];
}
