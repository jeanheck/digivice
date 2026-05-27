import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { DigievolutionViewModel } from "@/viewmodels/digievolution/digievolution.viewmodel";

export class DigimonSlotPresenter {
    public static getActiveDigievolutionViewModel(activeDigievolutionId: number): DigievolutionViewModel {
        return DigievolutionRepository.getRawDigievolutionById(activeDigievolutionId);
    }
}
