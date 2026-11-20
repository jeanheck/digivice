import type { Requisite } from "@/state/models";
import { PartyLevelHelper } from "@/presentation/presenters/helper/party-level.helper";
import type { RequisiteRaw } from "@/presentation/repositories/tables/raws/quest/requisite.raw";
import type { RequisiteViewModel } from "@/presentation/viewmodels/quest/requisite.viewmodel";

export class RequisiteConverter {
  public static convert(
    requisiteRaw: RequisiteRaw,
    requisites: Requisite[],
    partyLevel: number,
  ): RequisiteViewModel {
    if (requisiteRaw.type === "partyLevelRange") {
      const min = requisiteRaw.min ?? 0;
      const max = requisiteRaw.max ?? 0;

      return {
        id: requisiteRaw.id,
        isDone: PartyLevelHelper.isInRange(partyLevel, min, max),
      };
    }

    return {
      id: requisiteRaw.id,
      isDone: requisites.some((requisite) => {
        return requisite.id === requisiteRaw.id && requisite.isDone;
      }),
    };
  }
}
