import { DigievolutionTechniqueConverter } from "@/presentation/presenters/converter/digievolution-technique.converter";
import { TechniqueConverter } from "@/presentation/presenters/converter/technique.converter";
import { DigievolutionTechniquesHelper } from "@/presentation/presenters/helper/digievolution-techniques.helper";
import { DigievolutionRepository } from "@/presentation/repositories/digievolution.repository";
import { DigimonRepository } from "@/presentation/repositories/digimon.repository";
import type { RequirementViewModel } from "@/presentation/viewmodels/digimon/requirement.viewmodel";
import type { DigimonDigievolutionViewModel } from "@/presentation/viewmodels/digimon/digimon-digievolution.viewmodel";
import type { LinkViewModel } from "@/presentation/viewmodels/digievolution/link.viewmodel";
import type { DigievolutionTechniquesViewModel } from "@/presentation/viewmodels/digievolution/digievolution-techniques.viewmodel";
import type { TechniqueViewModel } from "@/presentation/viewmodels/digievolution/technique.viewmodel";

export interface DigievolutionTechniquesOptions {
  digimonId?: number;
  digievolutionLevel?: number;
  showTreeSections?: boolean;
}

export class DigievolutionTechniquesPresenter {
  public static getViewModel(
    digievolutionId: number,
    options: DigievolutionTechniquesOptions = {},
  ): DigievolutionTechniquesViewModel {
    const { digimonId, digievolutionLevel, showTreeSections = false } = options;
    const evolutionName = DigievolutionRepository.getNameById(digievolutionId);
    const techniques = this.getTechniquesByDigievolutionId(digievolutionId, digievolutionLevel);

    if (!showTreeSections || digimonId === undefined) {
      return {
        evolutionName,
        requirementDigievolutions: [],
        derivativeDigievolutions: [],
        techniques,
      };
    }

    const evolutionRequirements = DigimonRepository.getDigievolutionRequirements(
      digimonId,
      digievolutionId,
    );
    const digievolutionsByDigimon = DigimonRepository.getDigievolutionsById(digimonId);

    return {
      evolutionName,
      requirementDigievolutions: this.getRequirementDigievolutions(evolutionRequirements),
      derivativeDigievolutions: this.getDerivativeDigievolutions(
        digievolutionsByDigimon,
        digievolutionId,
      ),
      techniques,
    };
  }

  private static getRequirementDigievolutions(
    evolutionRequirements: RequirementViewModel[],
  ): LinkViewModel[] {
    return evolutionRequirements
      .filter((requirement) => requirement.type === "DigievolutionLevel")
      .map((requirement) => {
        const requirementDigievolutionId = requirement.digievolution!;

        return {
          id: requirementDigievolutionId,
          name: DigievolutionRepository.getNameById(requirementDigievolutionId),
        };
      });
  }

  private static getDerivativeDigievolutions(
    digievolutionsByDigimon: DigimonDigievolutionViewModel,
    digievolutionId: number,
  ): LinkViewModel[] {
    return Object.entries(digievolutionsByDigimon)
      .filter(([, requirements]) => {
        return requirements.some((requirement) => {
          if (requirement.type !== "DigievolutionLevel" || !requirement.digievolution) {
            return false;
          }

          return requirement.digievolution === digievolutionId;
        });
      })
      .map(([candidateDigievolutionId]) => {
        const id = Number(candidateDigievolutionId);

        return {
          id,
          name: DigievolutionRepository.getNameById(id),
        };
      });
  }

  private static getTechniquesByDigievolutionId(
    digievolutionId: number,
    digievolutionLevel?: number,
  ): TechniqueViewModel[] {
    const digievolutionTechniquesRaw =
      DigievolutionRepository.getRawDigievolutionTechniquesById(digievolutionId);

    if (digievolutionTechniquesRaw.length === 0) {
      return [];
    }

    const digievolutionTechniques = digievolutionTechniquesRaw.map((digievolutionTechniqueRaw) => {
      return DigievolutionTechniqueConverter.convert(digievolutionTechniqueRaw);
    });
    const signatureTechniqueId =
      DigievolutionTechniquesHelper.getSignatureTechniqueId(digievolutionTechniques);

    return digievolutionTechniques.map((digievolutionTechnique) => {
      const techniqueRaw = DigievolutionRepository.getTechniqueById(digievolutionTechnique.id);
      const isSignature = signatureTechniqueId === digievolutionTechnique.id;

      return TechniqueConverter.convert(
        digievolutionTechnique,
        techniqueRaw,
        isSignature,
        digievolutionLevel,
      );
    });
  }
}
