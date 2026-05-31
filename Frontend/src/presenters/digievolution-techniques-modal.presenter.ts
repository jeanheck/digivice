import { DigievolutionTechniqueConverter } from "@/presenters/converter/digievolution-technique.converter";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { DigievolutionTechniqueViewModel } from "@/viewmodels/digievolution/digievolution-technique.viewmodel";

export class DigievolutionTechniquesModalPresenter {
    public static getTechniquesByDigievolutionId(digievolutionId: number): DigievolutionTechniqueViewModel[] {
        const digievolutionTechniquesRaw = DigievolutionRepository.getRawDigievolutionTechniquesById(digievolutionId);

        return digievolutionTechniquesRaw.map((digievolutionTechniqueRaw) => {
            return DigievolutionTechniqueConverter.convert(digievolutionTechniqueRaw);
        });
    }

    public static getSignatureTechnique(digievolutionTechniques: DigievolutionTechniqueViewModel[]): string {
        const lastTechniqueToBeLearned = digievolutionTechniques.reduce((highest, current) => {
            return current.learnLevel > highest.learnLevel ? current : highest;
        });

        return lastTechniqueToBeLearned.id;
    }
}
