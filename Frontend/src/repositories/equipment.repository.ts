import EquipmentJson from "@/database/equipment/equipment.json";
import type { EquipmentTable } from "./tables/equipment/equipment.table";
import type { EquipmentRaw } from "./tables/raws/equipment/equipment.raw";

export class EquipmentRepository {
  private static readonly equipmentTable = EquipmentJson as EquipmentTable;

  public static getEquipmentById(equipmentId: number): EquipmentRaw {
    return this.equipmentTable[equipmentId]!;
  }

  public static getEquipmentsByIds(equipmentIds: number[]): EquipmentRaw[] {
    return equipmentIds.map((equipmentId) => this.equipmentTable[equipmentId]!);
  }
}
