import type { Elements } from "@/models";
import { StatConverter } from "@/presenters/converter/stat.converter";
import type { ElementsViewModel } from "@/viewmodels/digimon/elements.viewmodel";
import type { DigievolutionElementsViewModel } from "@/viewmodels/digievolution/digievolution-elements.viewmodel";

export type ElementsEquipmentBonuses = Record<keyof Elements, number>;

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
        digievolutionElements?.fire ?? 0,
      ),
      water: StatConverter.convert(
        elements.water,
        equipmentBonuses.water,
        digievolutionElements?.water ?? 0,
      ),
      ice: StatConverter.convert(
        elements.ice,
        equipmentBonuses.ice,
        digievolutionElements?.ice ?? 0,
      ),
      wind: StatConverter.convert(
        elements.wind,
        equipmentBonuses.wind,
        digievolutionElements?.wind ?? 0,
      ),
      thunder: StatConverter.convert(
        elements.thunder,
        equipmentBonuses.thunder,
        digievolutionElements?.thunder ?? 0,
      ),
      machine: StatConverter.convert(
        elements.machine,
        equipmentBonuses.machine,
        digievolutionElements?.machine ?? 0,
      ),
      dark: StatConverter.convert(
        elements.dark,
        equipmentBonuses.dark,
        digievolutionElements?.dark ?? 0,
      ),
    };
  }
}
