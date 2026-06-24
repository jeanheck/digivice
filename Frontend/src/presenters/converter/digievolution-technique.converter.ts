import type { DigievolutionTechniqueRaw } from "@/repositories/tables/raws/digievolution/digievolution-technique.raw";
import type { DigievolutionTechniqueViewModel } from "@/viewmodels/digievolution/digievolution-technique.viewmodel";

export class DigievolutionTechniqueConverter {
    public static convert(digievolutionTechniqueRaw: DigievolutionTechniqueRaw): DigievolutionTechniqueViewModel {
        return {
            id: digievolutionTechniqueRaw.id,
            learnLevel: digievolutionTechniqueRaw.learnLevel,
            loadedLevel: digievolutionTechniqueRaw.loadedLevel,
        };
    }
}
