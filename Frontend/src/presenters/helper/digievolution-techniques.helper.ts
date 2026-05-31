import type { DigievolutionTechniqueViewModel } from "@/viewmodels/digievolution/digievolution-technique.viewmodel";

export class DigievolutionTechniquesHelper {
    public static getSignatureTechniqueId(digievolutionTechniques: DigievolutionTechniqueViewModel[]): string {
        const lastTechniqueToBeLearned = digievolutionTechniques.reduce((highest, current) => {
            return current.learnLevel > highest.learnLevel ? current : highest;
        });

        return lastTechniqueToBeLearned.id;
    }
}
