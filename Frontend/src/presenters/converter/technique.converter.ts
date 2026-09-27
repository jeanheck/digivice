import type { TechniqueRaw } from "@/repositories/tables/raws/digievolution/technique.raw";
import type { DigievolutionTechniqueViewModel } from "@/viewmodels/digievolution/digievolution-technique.viewmodel";
import type { TechniqueViewModel } from "@/viewmodels/digievolution/technique.viewmodel";

export class TechniqueConverter {
  public static convert(
    digievolutionTechnique: DigievolutionTechniqueViewModel,
    techniqueRaw: TechniqueRaw,
    isSignature: boolean,
    digievolutionLevel?: number,
  ): TechniqueViewModel {
    const isUnlocked =
      digievolutionLevel === undefined
        ? true
        : digievolutionTechnique.learnAt <= digievolutionLevel;

    return {
      id: digievolutionTechnique.id,
      learnAt: digievolutionTechnique.learnAt,
      loadAt: digievolutionTechnique.loadAt,
      type: techniqueRaw.type,
      element: techniqueRaw.element,
      elementStrength: techniqueRaw.elementStrength,
      mp: techniqueRaw.mp,
      power: techniqueRaw.power,
      isUnlocked,
      isSignature,
    };
  }
}
