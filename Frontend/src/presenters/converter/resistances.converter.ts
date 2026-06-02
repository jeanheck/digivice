import type { Resistances } from "@/models/party/digimon/resistances";
import { StatConverter } from "@/presenters/converter/stat.converter";
import type { ResistancesViewModel } from "@/viewmodels/digimon/resistances.viewmodel";
import type { DigievolutionResistancesViewModel } from "@/viewmodels/digievolution/digievolution-resistances.viewmodel";

export type ResistancesEquipmentBonuses = Record<keyof Resistances, number>;

export class ResistancesConverter {
    public static convert(
        resistances: Resistances,
        digievolutionResistances: DigievolutionResistancesViewModel | null,
        equipmentBonuses: ResistancesEquipmentBonuses
    ): ResistancesViewModel {
        return {
            fire: StatConverter.convert(
                resistances.fire,
                equipmentBonuses.fire,
                digievolutionResistances?.fire ?? 0
            ),
            water: StatConverter.convert(
                resistances.water,
                equipmentBonuses.water,
                digievolutionResistances?.water ?? 0
            ),
            ice: StatConverter.convert(
                resistances.ice,
                equipmentBonuses.ice,
                digievolutionResistances?.ice ?? 0
            ),
            wind: StatConverter.convert(
                resistances.wind,
                equipmentBonuses.wind,
                digievolutionResistances?.wind ?? 0
            ),
            thunder: StatConverter.convert(
                resistances.thunder,
                equipmentBonuses.thunder,
                digievolutionResistances?.thunder ?? 0
            ),
            machine: StatConverter.convert(
                resistances.machine,
                equipmentBonuses.machine,
                digievolutionResistances?.machine ?? 0
            ),
            dark: StatConverter.convert(
                resistances.dark,
                equipmentBonuses.dark,
                digievolutionResistances?.dark ?? 0
            ),
        };
    }
}
