import { DigievolutionRepository } from "@/repositories/digievolution-repository";
import type { DigievolutionTechniqueRaw } from "@/repositories/tables/raws/digievolution/digievolution-technique-raw";

export class DigievolutionTechniquesModalPresenter {
    public static getTechniquesByDigievolutionId(digievolutionId: number): DigievolutionTechniqueRaw[] {
        return DigievolutionRepository.getRawDigievolutionTechniquesById(digievolutionId);
    }
    public static getSignatureTechnique(digievolutionTechniquesRaw: DigievolutionTechniqueRaw[]): string {
        const lastTechniqueToBeLearned = digievolutionTechniquesRaw.reduce((highest, current) =>
            current.learnLevel > highest.learnLevel ? current : highest
        );
        return lastTechniqueToBeLearned.id;
    }
}
