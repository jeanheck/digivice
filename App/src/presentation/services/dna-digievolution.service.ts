import type { Party } from "@/state/models";
import { DnaDigievolutionRepository } from "@/presentation/repositories/dna-digievolution.repository";
import type { DnaDigievolutionRaw } from "@/presentation/repositories/tables/raws/digievolution/dna-digievolution.raw";

interface DnaFusionPartner {
  digimonId: number;
  digievolutionId: number;
}

interface AvailableDnaFusion {
  dnaDigievolutionId: string;
  dnaDigievolution: DnaDigievolutionRaw;
  partners: DnaFusionPartner[];
}

export class DnaDigievolutionService {
  public static hasAvailableFusion(
    party: Party,
    digimonSlotIndex: number,
    digievolutionId: number,
  ): boolean {
    return (
      DnaDigievolutionService.getAvailableFusions(party, digimonSlotIndex, digievolutionId).length >
      0
    );
  }

  public static getAvailableFusions(
    party: Party,
    digimonSlotIndex: number,
    digievolutionId: number,
  ): AvailableDnaFusion[] {
    const otherDigievolutions = DnaDigievolutionService.getOtherDigievolutions(
      party,
      digimonSlotIndex,
    );

    return DnaDigievolutionRepository.getAll()
      .map(({ id, dnaDigievolution }) => {
        const partners = dnaDigievolution.digievolutions.flatMap(
          ([firstDigievolutionId, secondDigievolutionId]) => {
            const partnerDigievolutionId = DnaDigievolutionService.getPartnerDigievolutionId(
              digievolutionId,
              firstDigievolutionId,
              secondDigievolutionId,
            );
            if (partnerDigievolutionId === null) {
              return [];
            }

            return otherDigievolutions.filter(
              (otherDigievolution) =>
                otherDigievolution.digievolutionId === partnerDigievolutionId,
            );
          },
        );

        return { dnaDigievolutionId: id, dnaDigievolution, partners };
      })
      .filter((availableFusion) => availableFusion.partners.length > 0);
  }

  private static getOtherDigievolutions(
    party: Party,
    digimonSlotIndex: number,
  ): DnaFusionPartner[] {
    return party.slots
      .filter((slot) => slot.index !== digimonSlotIndex && slot.digimon !== null)
      .flatMap((slot) => {
        return slot
          .digimon!.digievolutions.filter(
            (digievolutionSlot) => digievolutionSlot.digievolutionId !== null,
          )
          .map((digievolutionSlot) => {
            return {
              digimonId: slot.digimonId!,
              digievolutionId: digievolutionSlot.digievolutionId!,
            };
          });
      });
  }

  private static getPartnerDigievolutionId(
    digievolutionId: number,
    firstDigievolutionId: number,
    secondDigievolutionId: number,
  ): number | null {
    if (firstDigievolutionId === digievolutionId) {
      return secondDigievolutionId;
    }
    if (secondDigievolutionId === digievolutionId) {
      return firstDigievolutionId;
    }

    return null;
  }
}
