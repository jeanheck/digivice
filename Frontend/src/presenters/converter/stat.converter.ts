import { StatCapHelper } from "@/presenters/helper/stat-cap.helper";
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
      sumBetweenDigimonAndEquipaments: StatCapHelper.capBasePlusEquip(fromDigimon, fromEquipaments),
    };
  }
}
