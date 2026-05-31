import type { Resistances } from "@/models/resistances";
import { Stat } from "@/models/stat";
import { StatConverter } from "@/presenters/converter/stat.converter";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { ResistancesViewModel } from "@/viewmodels/digimon/resistances.viewmodel";
import type { DigievolutionResistancesViewModel } from "@/viewmodels/digievolution/digievolution-resistances.viewmodel";

export class ResistancesConverter {
    public static convert(
        resistances: Resistances,
        digievolutionResistances: DigievolutionResistancesViewModel | null,
        rawEquipments: EquipmentRaw[]
    ): ResistancesViewModel {
        return {
            fire: StatConverter.convert(
                Stat.fire,
                resistances.fire,
                digievolutionResistances?.fire ?? 0,
                rawEquipments
            ),
            water: StatConverter.convert(
                Stat.water,
                resistances.water,
                digievolutionResistances?.water ?? 0,
                rawEquipments
            ),
            ice: StatConverter.convert(
                Stat.ice,
                resistances.ice,
                digievolutionResistances?.ice ?? 0,
                rawEquipments
            ),
            wind: StatConverter.convert(
                Stat.wind,
                resistances.wind,
                digievolutionResistances?.wind ?? 0,
                rawEquipments
            ),
            thunder: StatConverter.convert(
                Stat.thunder,
                resistances.thunder,
                digievolutionResistances?.thunder ?? 0,
                rawEquipments
            ),
            machine: StatConverter.convert(
                Stat.machine,
                resistances.machine,
                digievolutionResistances?.machine ?? 0,
                rawEquipments
            ),
            dark: StatConverter.convert(
                Stat.dark,
                resistances.dark,
                digievolutionResistances?.dark ?? 0,
                rawEquipments
            ),
        };
    }
}
