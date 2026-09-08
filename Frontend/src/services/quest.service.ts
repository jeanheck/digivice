import type { Quest } from "@/models";
import type { NpcMainQuestStepDoneRaw } from "@/repositories/tables/raws/npc/npc-main-quest-step-done.raw";
import type { QuestRaw } from "@/repositories/tables/raws/quest/quest.raw";

export class QuestService {
  public static getLastCompletedMainQuestStep(mainQuest: Quest | null): number {
    if (mainQuest === null) {
      return 0;
    }

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
    mainQuestStepDone?: NpcMainQuestStepDoneRaw,
  ): boolean {
    if (mainQuestStepDone === undefined) {
      return true;
    }

    if (lastCompletedMainQuestStep < mainQuestStepDone.min) {
      return false;
    }

    if (
      mainQuestStepDone.max !== undefined &&
      lastCompletedMainQuestStep >= mainQuestStepDone.max
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
