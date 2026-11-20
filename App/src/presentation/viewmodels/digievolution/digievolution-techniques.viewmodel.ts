import type { LinkViewModel } from "@/presentation/viewmodels/digievolution/link.viewmodel";
import type { TechniqueViewModel } from "@/presentation/viewmodels/digievolution/technique.viewmodel";

export interface DigievolutionTechniquesViewModel {
  evolutionName: string;
  requirementDigievolutions: LinkViewModel[];
  derivativeDigievolutions: LinkViewModel[];
  techniques: TechniqueViewModel[];
}
