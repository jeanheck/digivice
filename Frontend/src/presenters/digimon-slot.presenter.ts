import { DigievolutionRepository } from "@/repositories/digievolution-repository";
import type { DigievolutionViewModel } from "@/view-models/digievolution-view-model";

export class DigimonSlotPresenter {
    public static getActiveDigievolutionViewModel(activeDigievolutionId: number): DigievolutionViewModel {
        return DigievolutionRepository.getRawDigievolution(activeDigievolutionId);
    }
}
