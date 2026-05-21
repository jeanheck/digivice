import type { EnrichedEquipment, DigimonStatusType } from '../../models';
import { EquipamentType } from '../../models';
import { Repository } from '../../repositories/repository';

export class EquipmentsHelper {
    public static resolveEquipment(id: number | null): EnrichedEquipment | null {
        return Repository.getEquipmentById(id);
    }


    public static getEquipmentsWithoutDuplication(activeEquipments: EnrichedEquipment[]): EnrichedEquipment[] {
        return activeEquipments.filter((item, index, self) => {
            if (item.type === EquipamentType.WeaponTwoHanded) {
                return self.findIndex((i: EnrichedEquipment) => {
                    return i.id === item.id;
                }) === index;
            }
            return true;
        });
    }

    public static getEquipmentsThatAffects(
        activeEquipments: EnrichedEquipment[],
        digimonStatusType: DigimonStatusType
    ): EnrichedEquipment[] {
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
