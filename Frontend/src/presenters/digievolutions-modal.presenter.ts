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
    public static getAllDigievolutionsNames(): string[] {
        return DigievolutionRepository.getAllDigievolutionsNames();
    }
    public static getDigievolutionNameById(digievolutionId: number): string {
        return DigievolutionRepository.getNameById(digievolutionId);
    }
    public static getDigievolutionIdByName(digievolutionName: string): number {
        return DigievolutionRepository.getIdByName(digievolutionName);
    }
    public static getDigievolutionRequirements(
        digimonId: number,
        digievolutionId: number
    ): DigimonDigievolutionRequirementViewModel[] {
        return DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionId);
    }
    public static getDigievolutionRequirementsByName(
        digimonId: number,
        digievolutionName: string
    ): DigimonDigievolutionRequirementViewModel[] {
        const digievolutionId = DigievolutionRepository.getIdByName(digievolutionName);

        return this.getDigievolutionRequirements(digimonId, digievolutionId);
    }
}
