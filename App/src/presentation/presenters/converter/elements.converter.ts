import type { Elements } from "@/state/models";
import { StatConverter } from "@/presentation/presenters/converter/stat.converter";
import type { ElementsViewModel } from "@/presentation/viewmodels/digimon/elements.viewmodel";
import type { DigievolutionElementsViewModel } from "@/presentation/viewmodels/digievolution/digievolution-elements.viewmodel";

export type ElementsEquipmentBonuses = Record<keyof Elements, number>;

const EquipmentSetElementBonus = 0;

export class ElementsConverter {
  public static convert(
    elements: Elements,
    digievolutionElements: DigievolutionElementsViewModel | null,
    equipmentBonuses: ElementsEquipmentBonuses,
  ): ElementsViewModel {
    return {
      fire: StatConverter.convert(
        elements.fire,
        equipmentBonuses.fire,
        EquipmentSetElementBonus,
        digievolutionElements?.fire ?? 0,
      ),
      water: StatConverter.convert(
        elements.water,
        equipmentBonuses.water,
        EquipmentSetElementBonus,
        digievolutionElements?.water ?? 0,
      ),
      ice: StatConverter.convert(
        elements.ice,
        equipmentBonuses.ice,
        EquipmentSetElementBonus,
        digievolutionElements?.ice ?? 0,
      ),
      wind: StatConverter.convert(
        elements.wind,
        equipmentBonuses.wind,
        EquipmentSetElementBonus,
        digievolutionElements?.wind ?? 0,
      ),
      thunder: StatConverter.convert(
        elements.thunder,
        equipmentBonuses.thunder,
        EquipmentSetElementBonus,
        digievolutionElements?.thunder ?? 0,
      ),
      machine: StatConverter.convert(
        elements.machine,
        equipmentBonuses.machine,
        EquipmentSetElementBonus,
        digievolutionElements?.machine ?? 0,
      ),
      dark: StatConverter.convert(
        elements.dark,
        equipmentBonuses.dark,
        EquipmentSetElementBonus,
        digievolutionElements?.dark ?? 0,
      ),
    };
  }
}
