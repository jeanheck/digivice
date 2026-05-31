import { Stat } from "@/models/stat";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";

export class StatConverter {
    public static convert(
        stat: Stat,
        fromDigimon: number,
        fromDigievolution: number,
        rawEquipments: EquipmentRaw[]
    ): StatViewModel {
        const fromEquipaments = EquipmentsHelper.calculateBonusFromEquipaments(stat, rawEquipments);

        return {
            fromDigimon,
            fromEquipaments,
            fromDigievolution,
            sumBetweenDigimonAndEquipaments: fromDigimon + fromEquipaments,
        };
    }
}
