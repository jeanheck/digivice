import { Stat } from "@/models/stat";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import { MathUtils } from "@/utils/MathUtils";
import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";

export class StatConverter {
    public static convert(
        stat: Stat,
        fromDigimon: number,
        fromDigievolution: number,
        rawEquipments: EquipmentRaw[]
    ): StatViewModel {
        const fromEquipaments = StatConverter.calculateBonusFromEquipaments(stat, rawEquipments);

        return {
            fromDigimon,
            fromEquipaments,
            fromDigievolution,
            sumBetweenDigimonAndEquipaments: fromDigimon + fromEquipaments,
        };
    }

    private static calculateBonusFromEquipaments(stat: Stat, rawEquipments: EquipmentRaw[]): number {
        const lowerCaseType = stat.toLowerCase();
        const attributesRaw = rawEquipments
            .flatMap((rawEquipment) => rawEquipment.attributes)
            .filter((attribute) => attribute.attribute.toLowerCase() === lowerCaseType);

        return MathUtils.Sum(attributesRaw.map((attribute) => {
            return Number(`${attribute.type}${attribute.value}`);
        }));
    }
}
