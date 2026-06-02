import { MathHelper } from "@/presenters/helper/math.helper";
import type { Vitals } from "@/models";
import type { VitalsViewModel } from "@/viewmodels/digimon/vitals.viewmodel";

export class VitalsPresenter {
    public static getViewModel(vitals: Vitals): VitalsViewModel {
        return {
            hpPercentage: MathHelper.calculatePercentage(vitals.currentHP, vitals.maxHP),
            mpPercentage: MathHelper.calculatePercentage(vitals.currentMP, vitals.maxMP),
        };
    }
}
