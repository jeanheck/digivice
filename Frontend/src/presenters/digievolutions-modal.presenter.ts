import { DigimonRepository } from "@/repositories";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type { DigimonDigievolutionViewModel } from "@/viewmodels/digimon/digimon-digievolution.viewmodel";

export class DigievolutionsModalPresenter {
    public static getNameById(id: number): string {
        return DigimonRepository.getNameById(id);
    }
    public static getDigievolutionsById(id: number): DigimonDigievolutionViewModel {
        return DigimonRepository.getDigievolutionsById(id);
    }
    // TODO - change digievolutionName by digievolutionId
    public static getDigievolutionRequirements(digimonId: number, digievolutionName: string): DigimonDigievolutionRequirementViewModel[] {
        return DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionName);
    }
}
