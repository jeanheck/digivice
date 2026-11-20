import type { ChapterRaw } from "@/presentation/repositories/tables/raws/quest/chapter.raw";
import type { ChapterViewModel } from "@/presentation/viewmodels/quest/chapter.viewmodel";
import type { StepViewModel } from "@/presentation/viewmodels/quest/step.viewmodel";

export class ChapterConverter {
  public static convert(
    chapterRaws: ChapterRaw[],
    steps: StepViewModel[],
    currentStep: StepViewModel | null,
  ): ChapterViewModel[] {
    if (chapterRaws.length === 0) {
      return [];
    }

    const lastChapterNumber = chapterRaws.at(-1)?.number ?? null;

    return chapterRaws.map((chapterRaw) => {
      const chapterSteps = steps.filter((step) => {
        const stepNumber = Number(step.number);
        return stepNumber >= chapterRaw.starts && stepNumber <= chapterRaw.ends;
      });
      const doneCount = chapterSteps.filter((step) => step.isDone).length;
      let isCurrent = chapterRaw.number === lastChapterNumber;
      if (currentStep !== null) {
        isCurrent = chapterSteps.includes(currentStep);
      }

      return {
        number: chapterRaw.number,
        steps: chapterSteps,
        isDone: chapterSteps.length > 0 && doneCount === chapterSteps.length,
        isCurrent,
        doneCount,
        totalCount: chapterSteps.length,
      };
    });
  }
}
