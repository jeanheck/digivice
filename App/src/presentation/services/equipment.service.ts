import type { Equipments } from "@/state/models";
import { EquipmentRepository } from "@/presentation/repositories/equipment.repository";
import { EquipmentSetRepository } from "@/presentation/repositories/equipment-set.repository";
import type { EquipmentRaw } from "@/presentation/repositories/tables/raws/equipment/equipment.raw";
import type { EquipmentAttributeRaw } from "@/presentation/repositories/tables/raws/equipment/equipment-attribute.raw";
import type { EquipmentSetRaw } from "@/presentation/repositories/tables/raws/equipment/equipment-set.raw";
import type { DigimonStat } from "@/shared/types/digimon-stat.type";

const TwoHandedWeaponType = "twoHandedWeapon";

export class EquipmentService {
  public static getEquipmentIds(equipments: Equipments): number[] {
    const equipmentIds: number[] = [];

    const pushIfEquipped = (equipmentId: number | null): void => {
      if (equipmentId !== null) {
        equipmentIds.push(equipmentId);
      }
    };

    pushIfEquipped(equipments.head);
    pushIfEquipped(equipments.body);
    pushIfEquipped(equipments.right);

    const isTwoHandedWeapon =
      equipments.right !== null &&
      EquipmentRepository.getEquipmentById(equipments.right).type === TwoHandedWeaponType;

    if (!isTwoHandedWeapon) {
      pushIfEquipped(equipments.left);
    }

    pushIfEquipped(equipments.accessory1);
    pushIfEquipped(equipments.accessory2);

    return equipmentIds;
  }

  public static calculateBonus(stat: DigimonStat, equipmentsRaws: EquipmentRaw[]): number {
    return EquipmentService.sumAttributes(
      stat,
      equipmentsRaws.flatMap((equipmentRaw) => equipmentRaw.attributes),
    );
  }

  public static getCompletedSet(
    digimonId: number,
    equipmentIds: number[],
  ): EquipmentSetRaw | null {
    const equipmentSet = EquipmentSetRepository.getByDigimonId(digimonId);
    if (!equipmentSet) {
      return null;
    }

    const isCompleted = equipmentSet.equipments.every((equipmentId) =>
      equipmentIds.includes(equipmentId),
    );

    return isCompleted ? equipmentSet : null;
  }

  public static calculateSetBonus(
    stat: DigimonStat,
    equipmentSet: EquipmentSetRaw | null,
  ): number {
    if (equipmentSet === null) {
      return 0;
    }

    return EquipmentService.sumAttributes(stat, equipmentSet.attributes);
  }

  private static sumAttributes(
    stat: DigimonStat,
    equipmentAttributeRaws: EquipmentAttributeRaw[],
  ): number {
    return Math.sum(
      equipmentAttributeRaws
        .filter((equipmentAttributeRaw) => equipmentAttributeRaw.attribute === stat)
        .map((equipmentAttributeRaw) => {
          return Number(`${equipmentAttributeRaw.type}${equipmentAttributeRaw.value}`);
        }),
    );
  }
}
