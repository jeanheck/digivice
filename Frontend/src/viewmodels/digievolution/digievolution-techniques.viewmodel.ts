import type { DigievolutionsModalDigievolutionLinkViewModel } from "@/viewmodels/digievolution/digievolutions-modal-digievolution-link.viewmodel";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";

export interface DigievolutionTechniquesViewModel {
    evolutionName: string;
    requirementDigievolutions: DigievolutionsModalDigievolutionLinkViewModel[];
    derivativeDigievolutions: DigievolutionsModalDigievolutionLinkViewModel[];
    techniques: TechniqueViewModel[];
}
