import { DigievolutionTechniqueConverter } from "@/presenters/converter/digievolution-technique.converter";
import { TechniqueConverter } from "@/presenters/converter/technique.converter";
import { DigievolutionTechniquesHelper } from "@/presenters/helper/digievolution-techniques.helper";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { DigimonRepository } from "@/repositories/digimon.repository";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type { DigimonDigievolutionViewModel } from "@/viewmodels/digimon/digimon-digievolution.viewmodel";
import type { DigievolutionsModalDigievolutionDetailsViewModel } from "@/viewmodels/digievolution/digievolutions-modal-digievolution-details.viewmodel";
import type { DigievolutionsModalDigievolutionLinkViewModel } from "@/viewmodels/digievolution/digievolutions-modal-digievolution-link.viewmodel";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";

export class DigievolutionsModalDigievolutionDetailsPresenter {
    public static getDetailPanelViewModel(
        digimonId: number,
        digievolutionId: number | undefined
    ): DigievolutionsModalDigievolutionDetailsViewModel | null {
        if (digievolutionId === undefined) {
            return null;
        }

        const evolutionRequirements = DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionId);
        const digievolutionsByDigimon = DigimonRepository.getDigievolutionsById(digimonId);

        return {
            evolutionName: DigievolutionRepository.getNameById(digievolutionId),
            requirementDigievolutions: this.getRequirementDigievolutions(evolutionRequirements),
            derivativeDigievolutions: this.getDerivativeDigievolutions(digievolutionsByDigimon, digievolutionId),
            techniques: this.getTechniquesByDigievolutionId(digievolutionId),
        };
    }

    private static getRequirementDigievolutions(
        evolutionRequirements: DigimonDigievolutionRequirementViewModel[]
    ): DigievolutionsModalDigievolutionLinkViewModel[] {
        return evolutionRequirements
            .filter((requirement) => requirement.type === "DigievolutionLevel")
            .map((requirement) => {
                const digievolutionId = DigievolutionRepository.getIdByName(requirement.digievolution!);

                return {
                    id: digievolutionId,
                    name: DigievolutionRepository.getNameById(digievolutionId),
                };
            });
    }

    private static getDerivativeDigievolutions(
        digievolutionsByDigimon: DigimonDigievolutionViewModel,
        digievolutionId: number
    ): DigievolutionsModalDigievolutionLinkViewModel[] {
        return Object.entries(digievolutionsByDigimon)
            .filter(([, requirements]) => {
                return requirements.some((requirement) => {
                    if (requirement.type !== "DigievolutionLevel" || !requirement.digievolution) {
                        return false;
                    }

                    return DigievolutionRepository.getIdByName(requirement.digievolution) === digievolutionId;
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
