import type { DnaDigievolutionFusionMemberViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution-fusion-member.viewmodel";
import type { TechniqueType } from "@/shared/types/technique-type.type";

export interface DnaDigievolutionViewModel {
  id: string;
  name: string;
  imageUrl: string | null;
  fusions: [DnaDigievolutionFusionMemberViewModel, DnaDigievolutionFusionMemberViewModel][];
  type: TechniqueType;
  typeImageUrl: string | null;
  typeKey: string;
  elementImageUrl: string | null;
  elementKey: string | null;
  mp: number;
  effectKey: string | null;
}
