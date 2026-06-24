import { DigievolutionRepository, DigimonRepository } from "@/repositories";
import type { RequirementViewModel } from "@/viewmodels/digimon/requirement.viewmodel";
import type { LinkViewModel } from "@/viewmodels/digievolution/link.viewmodel";

export class DigievolutionsModalPresenter {
    public static getNameById(id: number): string {
        return DigimonRepository.getNameById(id);
    }
    public static getAllDigievolutions(): LinkViewModel[] {
        return DigievolutionRepository.getAllDigievolutions();
    }
    public static getDigievolutionNameById(digievolutionId: number): string {
        return DigievolutionRepository.getNameById(digievolutionId);
    }
    public static getDigievolutionRequirements(
        digimonId: number,
        digievolutionId: number
    ): RequirementViewModel[] {
        return DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionId);
    }
}
