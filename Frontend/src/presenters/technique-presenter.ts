import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import type { TechniqueViewModel } from "@/view-models/technique-view-model";

export class TechniquePresenter {
    public static getTechniqueById(
        techniqueId: string,
        learnLevel: number,
        loadedLevel: number,
        digievolutionLevel: number,
        isSignature: boolean
    ): TechniqueViewModel {
        const techniqueRaw = DigievolutionRepository.getTechniqueById(techniqueId);
        const isUnlocked = learnLevel <= digievolutionLevel;

        return {
            id: techniqueId,
            learnLevel: learnLevel,
            loadedLevel: loadedLevel,
            type: techniqueRaw.type,
            element: techniqueRaw.element,
            elementStrength: techniqueRaw.elementStrength,
            mp: techniqueRaw.mp,
            power: techniqueRaw.power,
            isUnlocked: isUnlocked,
            isSignature: isSignature
        };
    }
}
