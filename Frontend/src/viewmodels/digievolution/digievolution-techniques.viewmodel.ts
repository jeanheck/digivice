import type { LinkViewModel } from "@/viewmodels/digievolution/link.viewmodel";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";

export interface DigievolutionTechniquesViewModel {
    evolutionName: string;
    requirementDigievolutions: LinkViewModel[];
    derivativeDigievolutions: LinkViewModel[];
    techniques: TechniqueViewModel[];
}
