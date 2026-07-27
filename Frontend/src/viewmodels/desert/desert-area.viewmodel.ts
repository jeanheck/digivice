import type { DesertAreaTypeViewModel } from "./desert-area-type.viewmodel";

export interface DesertAreaViewModel {
  label: string;
  type: DesertAreaTypeViewModel;
  note?: string;
}
