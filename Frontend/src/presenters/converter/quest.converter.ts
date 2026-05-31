import type { Quest } from "@/models";
import { RequisiteConverter } from "@/presenters/converter/requisite.converter";
import { StepConverter } from "@/presenters/converter/step.converter";
import type { QuestRaw } from "@/repositories/tables/raws/quest/quest.raw";
import type { QuestCardVariant, QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";

export interface QuestConvertOptions {
    calculateNewStatus: boolean;
}

export class QuestConverter {
    public static convert(questRaw: QuestRaw, quest: Quest, options: QuestConvertOptions): QuestViewModel {
        const requisites = questRaw.requisites.map((requisiteRaw) => {
            return RequisiteConverter.convert(requisiteRaw, quest.requisites);
        });
        const steps = Object.entries(questRaw.steps).map(([stepNumber, stepRaw]) => {
            return StepConverter.convert(stepNumber, stepRaw, quest.steps);
        });
        const isDone = steps.length > 0 && steps.every((step) => step.isDone);
        const isLocked = options.calculateNewStatus
            && requisites.length > 0
            && !requisites.every((requisite) => requisite.isDone);
        const firstStep = steps.find((step) => step.number === "1");
        const isNew = options.calculateNewStatus
            && !isLocked
            && !isDone
            && firstStep?.isDone === false;
        const currentStep = steps.find((step) => !step.isDone) ?? null;
        const cardVariant = QuestConverter.resolveCardVariant(isLocked, isDone, isNew);

        return {
            id: questRaw.id,
            requisites,
            steps,
            isDone,
            isLocked,
            isNew,
            currentStep,
            cardVariant,
        };
    }

    private static resolveCardVariant(isLocked: boolean, isDone: boolean, isNew: boolean): QuestCardVariant {
        if (isLocked) {
            return "locked";
        }

        if (isDone) {
            return "done";
        }

        if (isNew) {
            return "new";
        }

        return "active";
    }
}
