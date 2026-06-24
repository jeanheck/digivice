import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";

export class StatConverter {
    public static convert(
        fromDigimon: number,
        fromEquipaments: number,
        fromDigievolution: number
    ): StatViewModel {
        return {
            fromDigimon,
            fromEquipaments,
            fromDigievolution,
            sumBetweenDigimonAndEquipaments: fromDigimon + fromEquipaments,
        };
    }
}
