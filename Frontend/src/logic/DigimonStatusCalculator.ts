import { EquipamentType, EquipamentsAttributesOperationType, type DigimonStatusType, type Equipament } from "../models/Digimon";

export class DigimonStatusCalculator {
    public static calculateBonusFromEquipaments(
        digimonStatusType: DigimonStatusType,
        activeEquipaments: Equipament[]): number {
        let total = 0;

        const uniqueItems = activeEquipaments.filter((item, index, self) => {
            if (item.type === EquipamentType.WeaponTwoHanded) {
                return self.findIndex((i: Equipament) => i.id === item.id) === index;
            }
            return true;
        });

        const lowerCaseTarget = digimonStatusType.toLowerCase();

        uniqueItems.forEach(item => {
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
