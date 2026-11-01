import type { Party } from "@/state/models";
import { DnaDigievolutionRepository } from "@/presentation/repositories/dna-digievolution.repository";

export class DnaDigievolutionService {
  public static hasAvailableFusion(
    party: Party,
    digimonSlotIndex: number,
    digievolutionId: number,
  ): boolean {
    const otherDigievolutionIds = new Set(
      party.slots
        .filter((slot) => slot.index !== digimonSlotIndex && slot.digimon !== null)
        .flatMap((slot) => slot.digimon!.digievolutions)
        .map((digievolutionSlot) => digievolutionSlot.digievolutionId)
        .filter((otherDigievolutionId) => otherDigievolutionId !== null),
    );

    return DnaDigievolutionRepository.getAll().some(({ dnaDigievolution }) => {
      return dnaDigievolution.digievolutions.some(([firstDigievolutionId, secondDigievolutionId]) => {
        if (firstDigievolutionId === digievolutionId) {
          return otherDigievolutionIds.has(secondDigievolutionId);
        }
        if (secondDigievolutionId === digievolutionId) {
          return otherDigievolutionIds.has(firstDigievolutionId);
        }
        return false;
      });
    });
  }
}
