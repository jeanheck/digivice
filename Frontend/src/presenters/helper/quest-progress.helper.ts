import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

export class QuestProgressHelper {
    public static isStarterOwnedPattern(steps: StepViewModel[]): boolean {
        if (steps.length < 2) {
            return false;
        }

        const sortedSteps = [...steps].sort((leftStep, rightStep) => {
            return Number(leftStep.number) - Number(rightStep.number);
        });
        const firstStep = sortedSteps[0];
        const lastStep = sortedSteps[sortedSteps.length - 1];

        if (firstStep === undefined || lastStep === undefined) {
            return false;
        }

        return firstStep.isDone === false && lastStep.isDone === true;
    }
}
