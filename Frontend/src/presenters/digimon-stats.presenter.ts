import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { DigievolutionViewModel } from "@/viewmodels/digievolution/digievolution.viewmodel";

export class DigimonStatsPresenter {
    public static getDigievolutionById(digievolutionId: number): DigievolutionViewModel {
        const digievolutionRaw = DigievolutionRepository.getRawDigievolutionById(digievolutionId);

        return  {
            name: digievolutionRaw.name,
            attributes: digievolutionRaw.attributes,
            resistances: digievolutionRaw.resistances
        };
    }
}
