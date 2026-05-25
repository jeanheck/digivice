import { DigievolutionRepository } from "@/repositories/digievolution-repository";

export class DigimonDigievolutionPresenter {
    public static getDigievolutionNameById(digievolutionId: number): string {
        return DigievolutionRepository.getNameById(digievolutionId);
    }
}
