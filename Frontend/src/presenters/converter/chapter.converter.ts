import { createLogger } from "@/events/logger";
import type { ChapterRaw } from "@/repositories/tables/raws/quest/chapter.raw";
import type { ChapterViewModel } from "@/viewmodels/quest/chapter.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

const presenterLogger = createLogger("Presenter", "#e67e22");

export class ChapterConverter {
  private static readonly warnedQuestIds = new Set<string>();

  public static convert(
    questId: string,
    chapterRaws: ChapterRaw[],
    steps: StepViewModel[],
    currentStep: StepViewModel | null,
  ): ChapterViewModel[] {
    if (chapterRaws.length === 0) {
      return [];
    }

    ChapterConverter.warnUncoveredSteps(questId, chapterRaws, steps);

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

  private static warnUncoveredSteps(
    questId: string,
    chapterRaws: ChapterRaw[],
    steps: StepViewModel[],
  ): void {
    if (ChapterConverter.warnedQuestIds.has(questId)) {
      return;
    }

    const uncoveredStepNumbers = steps
      .map((step) => Number(step.number))
      .filter((stepNumber) => {
        return !chapterRaws.some((chapterRaw) => {
          return stepNumber >= chapterRaw.starts && stepNumber <= chapterRaw.ends;
        });
      });

    if (uncoveredStepNumbers.length === 0) {
      return;
    }

    ChapterConverter.warnedQuestIds.add(questId);
    presenterLogger.warn(
      `Quest "${questId}" has steps outside any chapter: ${uncoveredStepNumbers.join(", ")}`,
    );
  }
}
