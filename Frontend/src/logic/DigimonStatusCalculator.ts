import { MathUtils } from "@/utils/MathUtils";
import { AttributeType, ResistanceType, type EnrichedEquipment, type Equipments } from "@/models";
import { EquipmentRepository } from "@/repositories/equipment-repository";

export class DigimonStatusCalculator {
    public static calculateBonusFromEquipaments(attributeOrResistanceType: AttributeType | ResistanceType, enrichedEquipments: EnrichedEquipment[]
    ): number {
        const lowerCaseAttributeOrResistanceType = attributeOrResistanceType.toLowerCase();
        const attributes = enrichedEquipments
            .flatMap(enrichedEquipment => enrichedEquipment.attributes)
            .filter(attribute => attribute.attribute.toLowerCase() === lowerCaseAttributeOrResistanceType);

        return MathUtils.Sum(attributes.map(attribute => Number(`${attribute.type}${attribute.value}`)));
    }

    public static calculateBonusFromRawEquipments(
        attributeOrResistanceType: AttributeType | ResistanceType,
        equipments: Equipments
    ): number {
        const enrichedEquipments = EquipmentRepository.getEnrichedEquipmentsByIds(equipments);
        return this.calculateBonusFromEquipaments(attributeOrResistanceType, enrichedEquipments);
    }
}
