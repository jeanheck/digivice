import type { Attributes } from "@/models/attributes";
import { StatConverter } from "@/presenters/converter/stat.converter";
import type { AttributesViewModel } from "@/viewmodels/digimon/attributes.viewmodel";
import type { DigievolutionAttributesViewModel } from "@/viewmodels/digievolution/digievolution-attributes.viewmodel";

export type AttributesEquipmentBonuses = Record<keyof Attributes, number>;

export class AttributesConverter {
    public static convert(
        attributes: Attributes,
        digievolutionAttributes: DigievolutionAttributesViewModel | null,
        equipmentBonuses: AttributesEquipmentBonuses
    ): AttributesViewModel {
        return {
            strength: StatConverter.convert(
                attributes.strength,
                equipmentBonuses.strength,
                digievolutionAttributes?.strength ?? 0
            ),
            defense: StatConverter.convert(
                attributes.defense,
                equipmentBonuses.defense,
                digievolutionAttributes?.defense ?? 0
            ),
            spirit: StatConverter.convert(
                attributes.spirit,
                equipmentBonuses.spirit,
                digievolutionAttributes?.spirit ?? 0
            ),
            wisdom: StatConverter.convert(
                attributes.wisdom,
                equipmentBonuses.wisdom,
                digievolutionAttributes?.wisdom ?? 0
            ),
            speed: StatConverter.convert(
                attributes.speed,
                equipmentBonuses.speed,
                digievolutionAttributes?.speed ?? 0
            ),
            charisma: StatConverter.convert(
                attributes.charisma,
                equipmentBonuses.charisma,
                digievolutionAttributes?.charisma ?? 0
            ),
        };
    }
}
