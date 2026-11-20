import type { Attributes } from "@/state/models";
import { StatConverter } from "@/presentation/presenters/converter/stat.converter";
import type { AttributesViewModel } from "@/presentation/viewmodels/digimon/attributes.viewmodel";
import type { DigievolutionAttributesViewModel } from "@/presentation/viewmodels/digievolution/digievolution-attributes.viewmodel";

export type AttributesEquipmentBonuses = Record<keyof Attributes, number>;
export type AttributesEquipmentSetBonuses = Record<keyof Attributes, number>;

export class AttributesConverter {
  public static convert(
    attributes: Attributes,
    digievolutionAttributes: DigievolutionAttributesViewModel | null,
    equipmentBonuses: AttributesEquipmentBonuses,
    equipmentSetBonuses: AttributesEquipmentSetBonuses,
  ): AttributesViewModel {
    return {
      strength: StatConverter.convert(
        attributes.strength,
        equipmentBonuses.strength,
        equipmentSetBonuses.strength,
        digievolutionAttributes?.strength ?? 0,
      ),
      defense: StatConverter.convert(
        attributes.defense,
        equipmentBonuses.defense,
        equipmentSetBonuses.defense,
        digievolutionAttributes?.defense ?? 0,
      ),
      spirit: StatConverter.convert(
        attributes.spirit,
        equipmentBonuses.spirit,
        equipmentSetBonuses.spirit,
        digievolutionAttributes?.spirit ?? 0,
      ),
      wisdom: StatConverter.convert(
        attributes.wisdom,
        equipmentBonuses.wisdom,
        equipmentSetBonuses.wisdom,
        digievolutionAttributes?.wisdom ?? 0,
      ),
      speed: StatConverter.convert(
        attributes.speed,
        equipmentBonuses.speed,
        equipmentSetBonuses.speed,
        digievolutionAttributes?.speed ?? 0,
      ),
      charisma: StatConverter.convert(
        attributes.charisma,
        equipmentBonuses.charisma,
        equipmentSetBonuses.charisma,
        digievolutionAttributes?.charisma ?? 0,
      ),
    };
  }
}
