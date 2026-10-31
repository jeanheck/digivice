import { DigievolutionRepository } from "@/presentation/repositories/digievolution.repository";

export class DigievolutionPresenter {
  public static getNameById(digievolutionId: number): string {
    return DigievolutionRepository.getNameById(digievolutionId);
  }
}
