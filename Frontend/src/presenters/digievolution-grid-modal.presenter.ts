import { DigimonRepository } from "@/repositories";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon-digievolution-requirement.viewmodel";
import type { DigimonDigievolutionViewModel } from "@/viewmodels/digimon-digievolution.viewmodel";

export class DigievolutionGridModalPresenter {
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
