import EquipmentJson from '@/database/equipment/equipment.json';
import type { EquipmentTable } from './tables/equipment-table';
import type { EnrichedEquipment, Equipments } from '@/models';

export class EquipamentRepository {
    private static readonly equipmentTable = EquipmentJson as EquipmentTable;

    private static getNonRepeatedIds(ids: number[]): number[] {
        return [...new Set(ids)];
    }

    private static getEquipmentsIds(equipments: Equipments): number[] {
        const equipamentsIds = [
            equipments.head,
            equipments.body,
            equipments.rightHand,
            equipments.leftHand,
            equipments.accessory1,
            equipments.accessory2
        ].filter((id): id is number => id !== null && id !== undefined && id !== 0);

        return this.getNonRepeatedIds(equipamentsIds);
    }

    public static getEnrichedEquipmentsByIds(equipments: Equipments): EnrichedEquipment[] {
        const equipmentsIds = this.getEquipmentsIds(equipments);
        const enrichedEquipments = equipmentsIds.map(equipmentId => this.equipmentTable[equipmentId]!);

        return enrichedEquipments;
    }
}
