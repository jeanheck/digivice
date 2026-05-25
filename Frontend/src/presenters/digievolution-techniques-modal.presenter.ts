import { DigievolutionRepository } from "@/repositories/digievolution-repository";
import { TechniqueRepository } from "@/repositories/technique-repository";
import type { TechniqueViewModel } from "@/view-models/technique-view-model";

export class DigievolutionTechniquesModalPresenter {
    public static getTechniquesByDigievolutionId(digievolutionId: number): TechniqueViewModel[] {
        const digievolutionsTechniqueRaw = DigievolutionRepository.getRawDigievolutionTechniquesById(digievolutionId);
        const techniquesRaw = digievolutionsTechniqueRaw
            .map(digievolutionTechniqueRaw => TechniqueRepository.getTechniqueById(digievolutionTechniqueRaw.id));
        
        return techniquesRaw.map(techniqueRaw => ({
            type: techniqueRaw.type,
            element: techniqueRaw.element,
            elementStrength: techniqueRaw.elementStrength,
            mp: techniqueRaw.mp,
            power: techniqueRaw.power
        }));
    }
}
