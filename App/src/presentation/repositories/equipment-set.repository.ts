import EquipmentSetJson from "@/data/game/equipment-set.json";
import type { EquipmentSetTable } from "@/presentation/repositories/tables/equipment/equipment-set.table";
import type { EquipmentSetRaw } from "@/presentation/repositories/tables/raws/equipment/equipment-set.raw";

export class EquipmentSetRepository {
  private static readonly equipmentSetTable = EquipmentSetJson as EquipmentSetTable;

  public static getByDigimonId(digimonId: number): EquipmentSetRaw | undefined {
    return this.equipmentSetTable[String(digimonId)];
  }
}
