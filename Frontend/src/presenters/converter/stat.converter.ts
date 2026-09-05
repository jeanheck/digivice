import { StatHelper } from "@/helpers/stat.helper";
import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";

export class StatConverter {
  public static convert(
    fromDigimon: number,
    fromEquipaments: number,
    fromDigievolution: number,
  ): StatViewModel {
    return {
      fromDigimon,
      fromEquipaments,
      fromDigievolution,
      sumBetweenDigimonAndEquipaments: StatHelper.calculateStat(fromDigimon, fromEquipaments),
    };
  }
}
