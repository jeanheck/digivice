import { TechniqueConverter } from "@/presenters/converter/technique.converter";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { DigievolutionTechniqueViewModel } from "@/viewmodels/digievolution/digievolution-technique.viewmodel";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";

export class TechniquePresenter {
    public static getTechnique(
        digievolutionTechnique: DigievolutionTechniqueViewModel,
        isSignature: boolean,
        digievolutionLevel: number
    ): TechniqueViewModel {
        const techniqueRaw = DigievolutionRepository.getTechniqueById(digievolutionTechnique.id);

        return TechniqueConverter.convert(
            digievolutionTechnique,
            techniqueRaw,
            isSignature,
            digievolutionLevel
        );
    }
}
