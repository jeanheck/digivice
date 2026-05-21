import type { Equipament, DigimonStatusType } from '../../models';
import { EquipamentType } from '../../models';
import EquipmentsData from '../../database/Equipments.json';
import EquipmentsTypeTable from '../../database/EquipmentsTypeTable.json';

export class EquipmentsHelper {
    public static resolveEquipment(id: number | null): Equipament | null {
        if (id === null || id === undefined || id === 0) {
            return null;
        }

        const item = EquipmentsData.equipments.find((e: any) => {
            return e.Id === id;
        });

        if (!item) {
            return null;
        }

        const typeInfo = EquipmentsTypeTable.types.find((t: any) => {
            return t.Id === item.Type;
        });

        return {
            id: item.Id as number,
            name: item.Name as Record<string, string>,
            type: (item.Type as any) || EquipamentType.Unknown,
            typeDescription: typeInfo ? typeInfo.Description as Record<string, string> : null,
            attributes: item.Attributes ? item.Attributes.map((a: any) => {
                return {
                    attribute: a.Attribute.toLowerCase() as DigimonStatusType,
                    type: a.Type as any,
                    value: a.Value
                };
            }) : [],
            equipableDigimon: item.EquipableDigimon || [],
            note: (item as any).Note as Record<string, string> | undefined
        };
    }

    public static getEquipmentsWithoutDuplication(activeEquipments: Equipament[]): Equipament[] {
        return activeEquipments.filter((item, index, self) => {
            if (item.type === EquipamentType.WeaponTwoHanded) {
                return self.findIndex((i: Equipament) => {
                    return i.id === item.id;
                }) === index;
            }
            return true;
        });
    }

    public static getEquipmentsThatAffects(
        activeEquipments: Equipament[],
        digimonStatusType: DigimonStatusType
    ): Equipament[] {
        const lowerCaseTarget = digimonStatusType.toLowerCase();
        return activeEquipments.filter((item) => {
            if (!item.attributes) {
                return false;
            }
            return item.attributes.some((attr) => {
                return attr.attribute.toLowerCase() === lowerCaseTarget;
            });
        });
    }
}
