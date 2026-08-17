import type { WikiLocationSourceViewModel } from "@/viewmodels/wiki-modal/wiki-location-source.viewmodel";

export interface WikiLocationViewModel {
  id: string;
  labelKey: string;
  sources: WikiLocationSourceViewModel[];
}
