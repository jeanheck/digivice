export class DigimonStatusCalculator {
    public static getEquipBonus(targetProperty: string, equippedItems: any[]): number {
        let total = 0;
        
        const uniqueItems = equippedItems.filter((item, index, self) => {
            if (item.Type === "WeaponTwoHanded") {
                return self.findIndex((i: any) => i.Id === item.Id) === index;
            }
            return true;
        });

        const lowerCaseTarget = targetProperty.toLowerCase();

        uniqueItems.forEach(item => {
            if (item.Attributes) {
                item.Attributes.forEach((attr: any) => {
                    if (attr.Attribute?.toLowerCase() === lowerCaseTarget) {
                        if (attr.Type === "Addition") {
                            total += attr.Value;
                        } else if (attr.Type === "Subtraction") {
                            total -= attr.Value;
                        }
                    }
                });
            }
        });
        
        return total;
    }

    public static getDigiBonus(targetProperty: string, section: 'attributes' | 'resistances', digievolution: any | null): number {
        if (!digievolution) {
            return 0;
        }
        
        const jsonField = section === 'attributes' ? digievolution.Attributes : digievolution.Resistances;
        if (jsonField) {
            const pascalProp = targetProperty.charAt(0).toUpperCase() + targetProperty.slice(1);
            return jsonField[pascalProp] || 0;
        }
        
        return 0;
    }
}
