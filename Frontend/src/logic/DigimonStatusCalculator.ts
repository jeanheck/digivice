import { EquipamentsAttributesOperationType, type DigimonStatusType, type Equipament } from "../models";
import { EquipmentsHelper } from "./helpers/EquipmentsHelper";

export class DigimonStatusCalculator {
    public static calculateBonusFromEquipaments(
        digimonStatusType: DigimonStatusType,
        activeEquipaments: Equipament[]
    ): number {
        let total = 0;

        const uniqueItems = EquipmentsHelper.getEquipmentsWithoutDuplication(activeEquipaments);
        const affectingItems = EquipmentsHelper.getEquipmentsThatAffects(uniqueItems, digimonStatusType);
        const lowerCaseTarget = digimonStatusType.toLowerCase();

        affectingItems.forEach((item) => {
            if (item.attributes) {
                item.attributes.forEach((attr) => {
                    if (attr.attribute.toLowerCase() === lowerCaseTarget) {
                        if (attr.type === EquipamentsAttributesOperationType.Addition) {
                            total += attr.value;
                        } else if (attr.type === EquipamentsAttributesOperationType.Subtraction) {
                            total -= attr.value;
                        }
                    }
                });
            }
        });

        return total;
    }


    public static calculateBonusFromActiveDigievolution(
        digimonStatusType: DigimonStatusType,
        section: 'attributes' | 'resistances',
        digievolution: any | null): number {
        if (!digievolution) {
            return 0;
        }

        const jsonField = section === 'attributes' ? digievolution.Attributes : digievolution.Resistances;
        if (jsonField) {
            const pascalProp = digimonStatusType.charAt(0).toUpperCase() + digimonStatusType.slice(1);
            return jsonField[pascalProp] || 0;
        }

        return 0;
    }
}
