import type { EnrichedEquipment, DigimonStatusType } from '../../models';

export class EquipmentsHelper {
    public static getEquipmentsThatAffects(enrichedEquipments: EnrichedEquipment[], digimonStatusType: DigimonStatusType ): EnrichedEquipment[] {
        const lowerCaseTarget = digimonStatusType.toLowerCase();
        
        return enrichedEquipments.filter((item) => {
            if (!item.attributes) {
                return false;
            }
            return item.attributes.some((attr) => {
                return attr.attribute.toLowerCase() === lowerCaseTarget;
            });
        });
    }
}
