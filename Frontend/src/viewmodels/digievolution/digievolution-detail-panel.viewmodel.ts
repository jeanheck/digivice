import type { TechniqueViewModel } from "./technique.viewmodel";

export interface DigievolutionDetailPanelViewModel {
    evolutionName: string;
    requirementDigievolutionNames: string[];
    derivativeDigievolutionNames: string[];
    techniques: TechniqueViewModel[];
}
