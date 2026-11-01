import type { DnaDigievolutionViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-digievolution.viewmodel";
import type { DnaFusionPartnerViewModel } from "@/presentation/viewmodels/dna-digievolution/dna-fusion-partner.viewmodel";

export interface AvailableDnaFusionViewModel {
  dnaDigievolution: DnaDigievolutionViewModel;
  partners: DnaFusionPartnerViewModel[];
}
