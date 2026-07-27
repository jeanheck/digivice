import type { Quest } from "@/models";
import { RequisiteConverter } from "@/presenters/converter/requisite.converter";
import { StepConverter } from "@/presenters/converter/step.converter";
import { QuestProgressHelper } from "@/presenters/helper/quest-progress.helper";
import type { QuestRaw } from "@/repositories/tables/raws/quest/quest.raw";
import type { RequisiteRaw } from "@/repositories/tables/raws/quest/requisite.raw";
import type { QuestCardVariant, QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";
import type { RequisiteViewModel } from "@/viewmodels/quest/requisite.viewmodel";

export interface QuestConvertOptions {
    calculateNewStatus: boolean;
    partyLevel: number;
}

export class QuestConverter {
    public static convert(questRaw: QuestRaw, quest: Quest, options: QuestConvertOptions): QuestViewModel {
        const requisites = questRaw.requisites.map((requisiteRaw) => {
            return RequisiteConverter.convert(requisiteRaw, quest.requisites, options.partyLevel);
        });
        const steps = Object.entries(questRaw.steps).map(([stepNumber, stepRaw]) => {
            return StepConverter.convert(stepNumber, stepRaw, quest.steps, options.partyLevel);
        });
        const isUnavailable = QuestProgressHelper.isStarterOwnedPattern(steps);
        const isDone = !isUnavailable
            && steps.length > 0
            && steps.every((step) => step.isDone);
        const hasStarted = steps.some((step) => step.isDone);
        const isLocked = !isUnavailable
            && options.calculateNewStatus
            && requisites.length > 0
            && !QuestConverter.areRequisitesSatisfiedForLock(questRaw.requisites, requisites, hasStarted);
        const firstStep = steps.find((step) => step.number === "1");
        const isNew = options.calculateNewStatus
            && !isUnavailable
            && !isLocked
            && !isDone
            && firstStep?.isDone === false;
        const currentStep = steps.find((step) => !step.isDone) ?? null;
        const cardVariant = QuestConverter.resolveCardVariant(isUnavailable, isLocked, isDone, isNew);

        return {
            id: questRaw.id,
            requisites,
            steps,
            isDone,
            isLocked,
            isUnavailable,
            isNew,
            currentStep,
            cardVariant,
        };
    }

    private static areRequisitesSatisfiedForLock(
        requisiteRaws: RequisiteRaw[],
        requisites: RequisiteViewModel[],
        hasStarted: boolean
    ): boolean {
        return requisiteRaws.every((requisiteRaw, index) => {
            if (requisiteRaw.type === "partyLevelRange" && hasStarted) {
                return true;
            }

            return requisites[index]?.isDone === true;
        });
    }

    private static resolveCardVariant(
        isUnavailable: boolean,
        isLocked: boolean,
        isDone: boolean,
        isNew: boolean
    ): QuestCardVariant {
        if (isUnavailable) {
            return "unavailable";
        }

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
