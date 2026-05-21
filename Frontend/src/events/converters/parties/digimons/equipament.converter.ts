import type { DigimonStatusType, Equipament, EquipamentsAttributesOperationType, EquipamentType } from "@/models";

export class EquipamentConverter {
    /*public static convert(id: number): Equipament | null {
        if (!id || id === 0) {
            return null;
        }

        const item = EquipmentsData.equipments.find(e => e.Id === id);
        if (!item) {
            return null;
        }

        const typeInfo = EquipmentsTypeTable.types.find(t => t.Id === item.Type);

        return {
            id: item.Id as number,
            name: item.Name as Record<string, string>,
            type: (item.Type as EquipamentType) || EquipamentType.Unknown,
            typeDescription: typeInfo ? typeInfo.Description as Record<string, string> : null,
            attributes: item.Attributes ? item.Attributes.map((a: any) => ({
                attribute: a.Attribute.toLowerCase() as DigimonStatusType,
                type: a.Type as EquipamentsAttributesOperationType,
                value: a.Value
            })) : [],
            equipableDigimon: item.EquipableDigimon || [],
            note: (item as any).Note as Record<string, string> | undefined
        };
    }*/
}
