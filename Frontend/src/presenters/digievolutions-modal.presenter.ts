import { DigievolutionRepository, DigimonRepository } from "@/repositories";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type { DigimonDigievolutionViewModel } from "@/viewmodels/digimon/digimon-digievolution.viewmodel";

export class DigievolutionsModalPresenter {
    public static getNameById(id: number): string {
        return DigimonRepository.getNameById(id);
    }
    public static getDigievolutionsById(id: number): DigimonDigievolutionViewModel {
        return DigimonRepository.getDigievolutionsById(id);
    }
    public static getAllDigievolutionsNames() : string[] {
        return DigievolutionRepository.getAllDigievolutionsNames();
    }
    public static getDigievolutionRequirements(digimonId: number, digievolutionName: string): DigimonDigievolutionRequirementViewModel[] {
        const digievolutionId = DigievolutionRepository.getIdByName(digievolutionName);

        return DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionId);
    }
}
