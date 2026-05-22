import { EquipmentsAttributesOperationType, type EnrichedEquipment, type Equipments } from '../models';
import { EquipmentType } from '../models';
import EquipmentsData from '../database/Equipments.json';
import EquipmentsTypeTableData from '../database/EquipmentsTypeTable.json';

export class EquipamentRepository {
    private static _equipments: EnrichedEquipment[] = [];
    private static _equipmentsMap = new Map<number, EnrichedEquipment>();

    static {
        this.initializeEquipments();
    }

    private static initializeEquipments(): void {
        for (const item of EquipmentsData.equipments) {
            const typeInfo = EquipmentsTypeTableData.types.find((t) => {
                return t.Id === item.Type;
            });

            const resolved: EnrichedEquipment = {
                id: item.Id ?? 0,
                name: item.Name,
                type: (item.Type as EquipmentType) || EquipmentType.Unknown,
                typeDescription: typeInfo ? typeInfo.Description : null,
                attributes: item.Attributes ? item.Attributes.map((a: any) => {
                    return {
                        attribute: a.Attribute.toLowerCase() as any,
                        type: a.Type === "Addition" ? EquipmentsAttributesOperationType.Addition : EquipmentsAttributesOperationType.Subtraction,
                        value: a.Value
                    };
                }) : [],
                equipableDigimon: item.EquipableDigimon || [],
                note: (item as any).Note
            };

            this._equipments.push(resolved);
            this._equipmentsMap.set(resolved.id, resolved);
        }
    }

    public static get equipments(): EnrichedEquipment[] {
        return this._equipments;
    }

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

    public static getEquipmentsByIds(equipments: Equipments): EnrichedEquipment[] {
        const equipmentsIds = this.getEquipmentsIds(equipments);
        const enrichedEquipments = equipmentsIds
            .map((equipmentId) => this._equipmentsMap.get(equipmentId))
            .filter((enrichedEquipment): enrichedEquipment is EnrichedEquipment => {
                return enrichedEquipment !== undefined;
            });

        return enrichedEquipments;
    }
}
