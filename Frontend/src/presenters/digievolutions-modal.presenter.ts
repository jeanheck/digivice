import { DigievolutionRepository, DigimonRepository } from "@/repositories";
import type { DigimonDigievolutionRequirementViewModel } from "@/viewmodels/digimon/digimon-digievolution-requirement.viewmodel";
import type { DigievolutionsModalDigievolutionLinkViewModel } from "@/viewmodels/digievolution/digievolutions-modal-digievolution-link.viewmodel";

export class DigievolutionsModalPresenter {
    public static getNameById(id: number): string {
        return DigimonRepository.getNameById(id);
    }
    public static getAllDigievolutions(): DigievolutionsModalDigievolutionLinkViewModel[] {
        return DigievolutionRepository.getAllDigievolutions();
    }
    public static getDigievolutionNameById(digievolutionId: number): string {
        return DigievolutionRepository.getNameById(digievolutionId);
    }
    public static getDigievolutionRequirements(
        digimonId: number,
        digievolutionId: number
    ): DigimonDigievolutionRequirementViewModel[] {
        return DigimonRepository.getDigievolutionRequirements(digimonId, digievolutionId);
    }
}
