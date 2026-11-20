import type { ChapterRaw } from "./chapter.raw";
import type { RequisiteRaw } from "./requisite.raw";
import type { StepsRaw } from "./steps.raw";

export interface QuestRaw {
  id: string;
  requisites: RequisiteRaw[];
  chapters?: ChapterRaw[];
  steps: StepsRaw;
}
