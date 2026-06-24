import type { Requisite } from "@/models";
import type { RequisiteRaw } from "@/repositories/tables/raws/quest/requisite.raw";
import type { RequisiteViewModel } from "@/viewmodels/quest/requisite.viewmodel";

export class RequisiteConverter {
    public static convert(requisiteRaw: RequisiteRaw, requisites: Requisite[]): RequisiteViewModel {
        return {
            id: requisiteRaw.id,
            isDone: requisites.some((requisite) => {
                return requisite.id === requisiteRaw.id && requisite.isDone;
            }),
        };
    }
}
