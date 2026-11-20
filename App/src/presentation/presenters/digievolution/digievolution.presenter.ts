import type { Party } from "@/state/models";
import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { DnaDigievolutionConverter } from "@/presentation/presenters/converter/dna-digievolution.converter";
import { DigievolutionRepository } from "@/presentation/repositories/digievolution.repository";
import { DigimonRepository } from "@/presentation/repositories/digimon.repository";
import { DnaDigievolutionService } from "@/presentation/services/dna-digievolution.service";
import type { AvailableDnaFusionViewModel } from "@/presentation/viewmodels/dna-digievolution/available-dna-fusion.viewmodel";
import type { DigimonElement } from "@/shared/constants/digimon-element.constant";

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

  public static getAvailableDnaFusions(
    party: Party,
    digimonSlotIndex: number,
    digievolutionId: number,
  ): AvailableDnaFusionViewModel[] {
    return DnaDigievolutionService.getAvailableFusions(party, digimonSlotIndex, digievolutionId).map(
      ({ dnaDigievolutionId, dnaDigievolution, partners }) => {
        return {
          dnaDigievolution: DnaDigievolutionConverter.convert(
            dnaDigievolutionId,
            dnaDigievolution,
            {
              imageUrl: ImageCatalog.getDnaDigievolutionImageUrl(dnaDigievolution.name),
              typeImageUrl: ImageCatalog.getTechniqueTypeImageUrl(dnaDigievolution.type),
              elementImageUrl:
                dnaDigievolution.element === "none"
                  ? null
                  : ImageCatalog.getStatImageUrl(dnaDigievolution.element as DigimonElement),
            },
            [],
          ),
          partners: partners.map((partner) => {
            return {
              digimonId: partner.digimonId,
              digimonName: DigimonRepository.getNameById(partner.digimonId),
              digievolutionId: partner.digievolutionId,
              digievolutionName: DigievolutionRepository.getNameById(partner.digievolutionId),
            };
          }),
        };
      },
    );
  }
}
