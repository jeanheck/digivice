import { ImageCatalog } from "@/ui/catalogs/image.catalog";
import { DnaDigievolutionConverter } from "@/presentation/presenters/converter/dna-digievolution.converter";
import { DigievolutionRepository, DnaDigievolutionRepository } from "@/presentation/repositories";
import type { DnaDigievolutionFusionMemberViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution-fusion-member.viewmodel";
import type { DnaDigievolutionViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution.viewmodel";
import type { DigimonElement } from "@/shared/constants/digimon-element.constant";

export class DnaDigievolutionsModalPresenter {
  public static getViewModels(): DnaDigievolutionViewModel[] {
    const sortedDnaDigievolutions = DnaDigievolutionRepository.getAll().sort(
      (first, second) => first.dnaDigievolution.order - second.dnaDigievolution.order,
    );

    return sortedDnaDigievolutions.map(({ id, dnaDigievolution }) => {
      const fusions = dnaDigievolution.digievolutions.map(
        ([firstDigievolutionId, secondDigievolutionId]): [
          DnaDigievolutionFusionMemberViewModel,
          DnaDigievolutionFusionMemberViewModel,
        ] => {
          return [
            DnaDigievolutionsModalPresenter.getFusionMember(firstDigievolutionId),
            DnaDigievolutionsModalPresenter.getFusionMember(secondDigievolutionId),
          ];
        },
      );

      return DnaDigievolutionConverter.convert(
        id,
        dnaDigievolution,
        {
          imageUrl: ImageCatalog.getDigievolutionImageUrl(dnaDigievolution.name),
          typeImageUrl: ImageCatalog.getTechniqueTypeImageUrl(dnaDigievolution.type),
          elementImageUrl:
            dnaDigievolution.element === "none"
              ? null
              : ImageCatalog.getStatImageUrl(dnaDigievolution.element as DigimonElement),
        },
        fusions,
      );
    });
  }

  private static getFusionMember(digievolutionId: number): DnaDigievolutionFusionMemberViewModel {
    return {
      digievolutionId,
      name: DigievolutionRepository.getNameById(digievolutionId),
    };
  }
}
