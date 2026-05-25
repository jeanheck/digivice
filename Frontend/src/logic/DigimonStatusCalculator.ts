import { MathUtils } from "@/utils/MathUtils";
import { AttributeType, EquipmentsAttributesOperationType, ResistanceType, type Equipments } from "@/models";
import { EquipmentRepository } from "@/repositories/equipment-repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment-raw";

export class DigimonStatusCalculator {
    public static calculateBonusFromEquipaments(type: AttributeType | ResistanceType, rawEquipments: EquipmentRaw[]
    ): number {
        const lowerCaseType = type.toLowerCase();
        const attributesRaw = rawEquipments
            .flatMap(rawEquipment => rawEquipment.attributes)
            .filter(attribute => attribute.attribute.toLowerCase() === lowerCaseType);

        return MathUtils.Sum(attributesRaw.map(attribute => {
            const operation = attribute.type === EquipmentsAttributesOperationType.Addition ? "+" : "-";
            return Number(`${operation}${attribute.value}`);
        }));
    }

    public static calculateBonusFromRawEquipments(
        type: AttributeType | ResistanceType,
        equipments: Equipments
    ): number {
        

        const rawEquipments = EquipmentRepository.getRawEquipmentsByIds(equipments);
        return this.calculateBonusFromEquipaments(type, rawEquipments);
    }
}
