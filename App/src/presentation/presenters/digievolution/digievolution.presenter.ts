import type { Party } from "@/state/models";
import { DigievolutionRepository } from "@/presentation/repositories/digievolution.repository";
import { DnaDigievolutionService } from "@/presentation/services/dna-digievolution.service";

export class DigievolutionPresenter {
  public static getNameById(digievolutionId: number): string {
    return DigievolutionRepository.getNameById(digievolutionId);
  }

  public static hasAvailableDnaFusion(
    party: Party,
    digimonSlotIndex: number,
    digievolutionId: number,
  ): boolean {
    return DnaDigievolutionService.hasAvailableFusion(party, digimonSlotIndex, digievolutionId);
  }
}
