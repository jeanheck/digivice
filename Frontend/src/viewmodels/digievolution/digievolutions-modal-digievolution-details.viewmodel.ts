import type { DigievolutionsModalDigievolutionLinkViewModel } from "./digievolutions-modal-digievolution-link.viewmodel";
import type { TechniqueViewModel } from "./technique.viewmodel";

export interface DigievolutionsModalDigievolutionDetailsViewModel {
    evolutionName: string;
    requirementDigievolutions: DigievolutionsModalDigievolutionLinkViewModel[];
    derivativeDigievolutions: DigievolutionsModalDigievolutionLinkViewModel[];
    techniques: TechniqueViewModel[];
}
