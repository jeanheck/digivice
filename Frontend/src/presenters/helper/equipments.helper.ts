import { EquipmentConstant, EQUIPMENT_SLOT_KEYS } from "@/constants/equipment.constant";
import type { Equipments } from "@/models";
import { Constant } from "@/constants/constant";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import { MathHelper } from "@/presenters/helper/math.helper";

const WEAPON_TWO_HANDED_TYPE = "weaponTwoHanded";

export class EquipmentsHelper {
  public static getEquipmentIds(equipments: Equipments): number[] {
    return [
      equipments.head,
      equipments.body,
      equipments.right,
      equipments.left,
      equipments.accessory1,
      equipments.accessory2,
    ].filter((id): id is number => id !== null && id !== undefined && id !== 0);
  }

  public static getBonusCalculationEquipmentIds(
    equipments: Equipments,
    resolveEquipmentType: (equipmentId: number) => string
  ): number[] {
    const rightEquipmentId = equipments.right;
    const leftEquipmentId = equipments.left;
    const shouldSkipLeftHandMirror =
      rightEquipmentId !== null &&
      rightEquipmentId !== 0 &&
      leftEquipmentId !== null &&
      leftEquipmentId !== 0 &&
      rightEquipmentId === leftEquipmentId &&
      resolveEquipmentType(rightEquipmentId) === WEAPON_TWO_HANDED_TYPE;

    return EQUIPMENT_SLOT_KEYS
      .filter((slotKey) => {
        if (shouldSkipLeftHandMirror && slotKey === EquipmentConstant.left) {
          return false;
        }

        return true;
      })
      .map((slotKey) => equipments[slotKey])
      .filter((equipmentId): equipmentId is number => {
        return equipmentId !== null && equipmentId !== undefined && equipmentId !== 0;
      });
  }

  public static calculateBonusFromEquipaments(stat: Constant, rawEquipments: EquipmentRaw[]): number {
    const lowerCaseType = stat.toLowerCase();
    const attributesRaw = rawEquipments
      .flatMap((rawEquipment) => rawEquipment.attributes)
      .filter((attribute) => attribute.attribute.toLowerCase() === lowerCaseType);

    return MathHelper.sum(attributesRaw.map((attribute) => {
      return Number(`${attribute.type}${attribute.value}`);
    }));
  }
}
