import type { Resistances } from "@/models";
import { StatConverter } from "@/presenters/converter/stat.converter";
import type { ResistancesViewModel } from "@/viewmodels/digimon/resistances.viewmodel";
import type { DigievolutionElementsViewModel } from "@/viewmodels/digievolution/digievolution-elements.viewmodel";

export type ResistancesEquipmentBonuses = Record<keyof Resistances, number>;

export class ResistancesConverter {
  public static convert(
    resistances: Resistances,
    digievolutionElements: DigievolutionElementsViewModel | null,
    equipmentBonuses: ResistancesEquipmentBonuses,
  ): ResistancesViewModel {
    return {
      fire: StatConverter.convert(
        resistances.fire,
        equipmentBonuses.fire,
        digievolutionElements?.fire ?? 0,
      ),
      water: StatConverter.convert(
        resistances.water,
        equipmentBonuses.water,
        digievolutionElements?.water ?? 0,
      ),
      ice: StatConverter.convert(
        resistances.ice,
        equipmentBonuses.ice,
        digievolutionElements?.ice ?? 0,
      ),
      wind: StatConverter.convert(
        resistances.wind,
        equipmentBonuses.wind,
        digievolutionElements?.wind ?? 0,
      ),
      thunder: StatConverter.convert(
        resistances.thunder,
        equipmentBonuses.thunder,
        digievolutionElements?.thunder ?? 0,
      ),
      machine: StatConverter.convert(
        resistances.machine,
        equipmentBonuses.machine,
        digievolutionElements?.machine ?? 0,
      ),
      dark: StatConverter.convert(
        resistances.dark,
        equipmentBonuses.dark,
        digievolutionElements?.dark ?? 0,
      ),
    };
  }
}
