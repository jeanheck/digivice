import type { Quest } from "@/models";
import type { MainQuestAvailabilityWindowRaw } from "@/repositories/tables/raws/quest/main-quest-availability-window.raw";
import type { QuestRaw } from "@/repositories/tables/raws/quest/quest.raw";

export class QuestService {
  public static getLastCompletedMainQuestStep(mainQuest: Quest): number {
    const completedSteps = mainQuest.steps.filter((step) => {
      return step.isDone;
    });

    if (completedSteps.length === 0) {
      return 0;
    }

    return Math.max(
      ...completedSteps.map((step) => {
        return step.number;
      }),
    );
  }

  public static isOnMainQuestRange(
    lastCompletedMainQuestStep: number,
    mainQuestAvailabilityWindow?: MainQuestAvailabilityWindowRaw,
  ): boolean {
    if (mainQuestAvailabilityWindow === undefined) {
      return true;
    }

    if (lastCompletedMainQuestStep < mainQuestAvailabilityWindow.starts) {
      return false;
    }

    if (
      mainQuestAvailabilityWindow.ends !== undefined &&
      lastCompletedMainQuestStep >= mainQuestAvailabilityWindow.ends
    ) {
      return false;
    }

    return true;
  }

  public static isQuestCompleted(
    quest: Quest | null | undefined,
    questRaw: QuestRaw,
  ): boolean {
    if (quest === null || quest === undefined) {
      return false;
    }

    const stepNumbers = Object.keys(questRaw.steps);
    if (stepNumbers.length === 0) {
      return false;
    }

    return stepNumbers.every((stepNumber) => {
      return quest.steps.some((step) => {
        return step.number.toString() === stepNumber && step.isDone;
      });
    });
  }
}
