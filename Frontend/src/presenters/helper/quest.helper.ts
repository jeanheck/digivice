import type { Quest } from "@/models";

export class QuestHelper {
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

        return Math.max(...completedSteps.map((step) => {
            return step.number;
        }));
    }
}
