import { MathUtils } from "@/utils/MathUtils";
import { type Equipments } from "@/models";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { Stat } from "@/models/stat";

export class DigimonStatusCalculator {
    public static calculateBonusFromEquipaments(type: Stat, rawEquipments: EquipmentRaw[]
    ): number {
        const lowerCaseType = type.toLowerCase();
        const attributesRaw = rawEquipments
            .flatMap(rawEquipment => rawEquipment.attributes)
            .filter(attribute => attribute.attribute.toLowerCase() === lowerCaseType);

        return MathUtils.Sum(attributesRaw.map(attribute => {
            return Number(`${attribute.type}${attribute.value}`);
        }));
    }

    public static calculateBonusFromRawEquipments(
        type: Stat,
        equipments: Equipments
    ): number {
        

        const rawEquipments = EquipmentRepository.getRawEquipmentsByIds(equipments);
        return this.calculateBonusFromEquipaments(type, rawEquipments);
    }
}
