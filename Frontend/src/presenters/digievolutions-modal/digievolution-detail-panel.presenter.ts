import { DigievolutionTechniqueConverter } from "@/presenters/converter/digievolution-technique.converter";
import { TechniqueConverter } from "@/presenters/converter/technique.converter";
import { DigievolutionTechniquesHelper } from "@/presenters/helper/digievolution-techniques.helper";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type { DigimonDigievolutionViewModel } from "@/viewmodels/digimon/digimon-digievolution.viewmodel";
import type { DigievolutionDetailPanelViewModel } from "@/viewmodels/digievolution/digievolution-detail-panel.viewmodel";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";

export class DigievolutionDetailPanelPresenter {
    public static getDetailPanelViewModel(
        evolution: DigimonDigievolutionRequirementViewModel[],
        digievolutionId: number | undefined,
        derivativeParameter: DigimonDigievolutionViewModel
    ): DigievolutionDetailPanelViewModel | null {
        if (digievolutionId === undefined) {
            return null;
        }

        const evolutionName = DigievolutionRepository.getNameById(digievolutionId);

        return {
            evolutionName,
            requirementDigievolutionNames: this.getRequirementDigievolutionNames(evolution),
            derivativeDigievolutionNames: this.getDerivativeDigievolutionNames(derivativeParameter, evolutionName),
            techniques: this.getTechniquesByDigievolutionId(digievolutionId),
        };
    }

    private static getRequirementDigievolutionNames(
        evolution: DigimonDigievolutionRequirementViewModel[]
    ): string[] {
        return evolution
            .filter((requirement) => requirement.type === "DigievolutionLevel")
            .map((requirement) => requirement.digievolution!);
    }

    private static getDerivativeDigievolutionNames(
        derivativeParameter: DigimonDigievolutionViewModel,
        evolutionName: string
    ): string[] {
        const entries = Object.entries(derivativeParameter);

        const matchingEntries = entries.filter(([, requirements]) => {
            return requirements.some((requirement) => {
                return requirement.type === "DigievolutionLevel" && requirement.digievolution === evolutionName;
            });
        });

        return matchingEntries.map(([digievolutionIdKey]) => {
            return DigievolutionRepository.getNameById(Number(digievolutionIdKey));
        });
    }

    private static getTechniquesByDigievolutionId(digievolutionId: number): TechniqueViewModel[] {
        const digievolutionTechniquesRaw = DigievolutionRepository.getRawDigievolutionTechniquesById(digievolutionId);

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
