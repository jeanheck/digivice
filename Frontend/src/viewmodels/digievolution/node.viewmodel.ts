import type { RequirementViewModel } from "@/viewmodels/digimon/requirement.viewmodel";

export interface NodeViewModel {
  id: number;
  name: string;
  next: number | number[] | null;
  requirements: RequirementViewModel[];
}
