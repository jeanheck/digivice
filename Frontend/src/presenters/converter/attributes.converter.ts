import type { Attributes } from "@/models/attributes";
import { Stat } from "@/models/stat";
import { StatConverter } from "@/presenters/converter/stat.converter";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { AttributesViewModel } from "@/viewmodels/digimon/attributes.viewmodel";
import type { DigievolutionAttributesViewModel } from "@/viewmodels/digievolution/digievolution-attributes.viewmodel";

export class AttributesConverter {
    public static convert(
        attributes: Attributes,
        digievolutionAttributes: DigievolutionAttributesViewModel | null,
        rawEquipments: EquipmentRaw[]
    ): AttributesViewModel {
        return {
            strength: StatConverter.convert(
                Stat.strength,
                attributes.strength,
                digievolutionAttributes?.strength ?? 0,
                rawEquipments
            ),
            defense: StatConverter.convert(
                Stat.defense,
                attributes.defense,
                digievolutionAttributes?.defense ?? 0,
                rawEquipments
            ),
            spirit: StatConverter.convert(
                Stat.spirit,
                attributes.spirit,
                digievolutionAttributes?.spirit ?? 0,
                rawEquipments
            ),
            wisdom: StatConverter.convert(
                Stat.wisdom,
                attributes.wisdom,
                digievolutionAttributes?.wisdom ?? 0,
                rawEquipments
            ),
            speed: StatConverter.convert(
                Stat.speed,
                attributes.speed,
                digievolutionAttributes?.speed ?? 0,
                rawEquipments
            ),
            charisma: StatConverter.convert(
                Stat.charisma,
                attributes.charisma,
                digievolutionAttributes?.charisma ?? 0,
                rawEquipments
            ),
        };
    }
}
