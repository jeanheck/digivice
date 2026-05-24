import EquipmentJson from '@/database/equipment/equipment.json';
import type { EquipmentTable } from './tables/equipment-table';
import type { EnrichedEquipment, Equipments } from '@/models';

export class EquipmentRepository {
    private static readonly equipmentTable = EquipmentJson as EquipmentTable;

    private static getEquipmentsIds(equipments: Equipments): number[] {
        return [
            equipments.head,
            equipments.body,
            equipments.rightHand,
            equipments.leftHand,
            equipments.accessory1,
            equipments.accessory2
        ].filter((id): id is number => id !== null && id !== undefined && id !== 0);
    }
    private static getNonRepeatedEquipmentsIds(equipments: Equipments): number[] {
        const equipmentsIds = this.getEquipmentsIds(equipments);
        return [...new Set(equipmentsIds)];
    }

    public static getEnrichedEquipmentsByIds(equipments: Equipments): EnrichedEquipment[] {
        const nonRepeatedEquipmentsIds = this.getNonRepeatedEquipmentsIds(equipments);
        const enrichedEquipments = nonRepeatedEquipmentsIds.map(equipmentId => this.equipmentTable[equipmentId]!);

        return enrichedEquipments;
    }

    public static getEnrichedEquipmentById(equipamentId: number): EnrichedEquipment {
        return this.equipmentTable[equipamentId]!;
    }
}
