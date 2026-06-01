import { DigievolutionTechniqueConverter } from "@/presenters/converter/digievolution-technique.converter";
import { DigievolutionTechniquesHelper } from "@/presenters/helper/digievolution-techniques.helper";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { DigievolutionTechniqueViewModel } from "@/viewmodels/digievolution/digievolution-technique.viewmodel";

export class TechniquesModalPresenter {
    public static getTechniquesByDigievolutionId(digievolutionId: number): DigievolutionTechniqueViewModel[] {
        const digievolutionTechniquesRaw = DigievolutionRepository.getRawDigievolutionTechniquesById(digievolutionId);

        return digievolutionTechniquesRaw.map((digievolutionTechniqueRaw) => {
            return DigievolutionTechniqueConverter.convert(digievolutionTechniqueRaw);
        });
    }

    public static getSignatureTechniqueId(digievolutionTechniques: DigievolutionTechniqueViewModel[]): string {
        return DigievolutionTechniquesHelper.getSignatureTechniqueId(digievolutionTechniques);
    }
}
