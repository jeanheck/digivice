import type { StepViewModel } from "./step.viewmodel";

export interface ChapterViewModel {
  number: number;
  steps: StepViewModel[];
  isDone: boolean;
  isCurrent: boolean;
  doneCount: number;
  totalCount: number;
}
