import { DigievolutionTechniqueConverter } from "@/presenters/converter/digievolution-technique.converter";
import { TechniqueConverter } from "@/presenters/converter/technique.converter";
import { DigievolutionTechniquesHelper } from "@/presenters/helper/digievolution-techniques.helper";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";

export class DigievolutionDetailPanelPresenter {
    public static getTechniquesByEvolutionName(evolutionName: string): TechniqueViewModel[] {
        const digievolutionTechniquesRaw = DigievolutionRepository.getRawDigievolutionTechniquesByName(evolutionName);

        if (digievolutionTechniquesRaw.length === 0) {
            return [];
        }

        const digievolutionTechniques = digievolutionTechniquesRaw.map((digievolutionTechniqueRaw) => {
            return DigievolutionTechniqueConverter.convert(digievolutionTechniqueRaw);
        });
        const signatureTechniqueId = DigievolutionTechniquesHelper.getSignatureTechniqueId(digievolutionTechniques);

        return digievolutionTechniques.map((digievolutionTechnique) => {
            const techniqueRaw = DigievolutionRepository.getTechniqueById(digievolutionTechnique.id);
            const isSignature = signatureTechniqueId === digievolutionTechnique.id;

            return TechniqueConverter.convert(
                digievolutionTechnique,
                techniqueRaw,
                isSignature
            );
        });
    }
}
