import { MathUtils } from "@/utils/MathUtils";
import { AttributeType, ResistanceType, type EnrichedEquipment, type Equipments } from "@/models";
import { EquipamentRepository } from "@/repositories/equipament-repository";

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
        const enrichedEquipments = EquipamentRepository.getEnrichedEquipmentsByIds(equipments);
        return this.calculateBonusFromEquipaments(attributeOrResistanceType, enrichedEquipments);
    }


    public static calculateBonusFromActiveDigievolution(
        attributeOrResistanceType: AttributeType | ResistanceType,
        section: 'attributes' | 'resistances',
        digievolution: any | null): number {
        if (!digievolution) {
            return 0;
        }

        const jsonField = section === 'attributes' ? digievolution.Attributes : digievolution.Resistances;
        if (jsonField) {
            const pascalProp = attributeOrResistanceType.charAt(0).toUpperCase() + attributeOrResistanceType.slice(1);
            return jsonField[pascalProp] || 0;
        }

        return 0;
    }
}
