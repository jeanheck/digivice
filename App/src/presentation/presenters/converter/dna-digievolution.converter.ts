import type { DnaDigievolutionRaw } from "@/presentation/repositories/tables/raws/digievolution/dna-digievolution.raw";
import type { DnaDigievolutionFusionMemberViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution-fusion-member.viewmodel";
import type { DnaDigievolutionViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution.viewmodel";

export class DnaDigievolutionConverter {
  public static convert(
    id: string,
    dnaDigievolutionRaw: DnaDigievolutionRaw,
    imageUrls: {
      imageUrl: string | null;
      typeImageUrl: string | null;
      elementImageUrl: string | null;
    },
    fusions: [DnaDigievolutionFusionMemberViewModel, DnaDigievolutionFusionMemberViewModel][],
  ): DnaDigievolutionViewModel {
    return {
      id,
      name: dnaDigievolutionRaw.name,
      imageUrl: imageUrls.imageUrl,
      fusions,
      type: dnaDigievolutionRaw.type,
      typeImageUrl: imageUrls.typeImageUrl,
      typeKey: `techniqueTypes.${dnaDigievolutionRaw.type}`,
      elementImageUrl: imageUrls.elementImageUrl,
      elementKey: DnaDigievolutionConverter.getElementKey(dnaDigievolutionRaw.element),
      mp: dnaDigievolutionRaw.mp,
      effectKey:
        dnaDigievolutionRaw.effect === null
          ? null
          : `dnaDigievolution.effect.${dnaDigievolutionRaw.effect}`,
    };
  }

  private static getElementKey(element: string): string | null {
    if (element === "none") {
      return null;
    }

    return `stat.${element}`;
  }
}
