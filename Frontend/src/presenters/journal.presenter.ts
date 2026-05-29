import type { Journal, Quest, Requisite, Step } from "@/models";
import { QuestRepository } from "@/repositories/quest.repository";
import type { QuestRaw } from "@/repositories/tables/raws/quest/quest.raw";
import type { StepRaw } from "@/repositories/tables/raws/quest/step.raw";
import type { JournalViewModel } from "@/viewmodels/quest/journal.viewmodel";
import type { QuestCardVariant, QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";
import type { RequisiteViewModel } from "@/viewmodels/quest/requisite.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

interface QuestDisplayOptions {
    calculateNewStatus: boolean;
}

interface QuestViewModelBase {
    id: string;
    requisites: RequisiteViewModel[];
    steps: StepViewModel[];
}

export class JournalPresenter {
    public static getJournalViewModel(journal: Journal): JournalViewModel {
        const mainQuestRaw = QuestRepository.getMainQuestRaw();
        const mainQuestViewModel = journal.mainQuest === null
            ? null
            : this.enrichQuestViewModel(
                this.buildQuestViewModelBase(mainQuestRaw, journal.mainQuest),
                { calculateNewStatus: false }
            );

        const sideQuestsRaw = QuestRepository.getSideQuestsRaw();
        const sideQuestsViewModels = sideQuestsRaw
            .map(sideQuestRaw => this.enrichQuestViewModel(
                this.buildQuestViewModelBase(sideQuestRaw, this.getSideQuestFromJournal(journal, sideQuestRaw.id)),
                { calculateNewStatus: true }
            ));

        return {
            mainQuest: mainQuestViewModel,
            sideQuests: sideQuestsViewModels
        };
    }

    private static getSideQuestFromJournal(journal: Journal, sideQuestId: string): Quest {
        return journal.sideQuests.find(quest => quest.id === sideQuestId)!;
    }

    private static calculateIfRequisiteIsDone(requisiteRawId: string, requisites: Requisite[]): boolean {
        return requisites.some(requisite => requisite.id === requisiteRawId && requisite.isDone);
    }

    private static calculateIfStepIsDone(stepRawNumber: string, steps: Step[]): boolean {
        return steps.some(step => step.number.toString() === stepRawNumber && step.isDone);
    }

    private static pascalToCamelCase(value: string): string {
        if (!value) {
            return value;
        }

        return value.charAt(0).toLowerCase() + value.slice(1);
    }

    private static buildQuestViewModelBase(questRaw: QuestRaw, quest: Quest): QuestViewModelBase {
        return {
            id: this.pascalToCamelCase(questRaw.id),
            requisites: questRaw.requisites.map(requisiteRaw => ({
                id: requisiteRaw.id,
                isDone: this.calculateIfRequisiteIsDone(requisiteRaw.id, quest.requisites)
            })),
            steps: Object.entries(questRaw.steps).map(([number, stepRaw]) => {
                return this.convertStepRawToViewModel(number, stepRaw, quest.steps);
            })
        };
    }

    private static enrichQuestViewModel(quest: QuestViewModelBase, options: QuestDisplayOptions): QuestViewModel {
        const isDone = quest.steps.length > 0 && quest.steps.every(step => step.isDone);
        const isLocked = options.calculateNewStatus
            && quest.requisites.length > 0
            && !quest.requisites.every(requisite => requisite.isDone);
        const firstStep = quest.steps.find(step => step.number === "1");
        const isNew = options.calculateNewStatus
            && !isLocked
            && !isDone
            && firstStep?.isDone === false;
        const currentStep = quest.steps.find(step => !step.isDone) ?? null;
        const cardVariant = this.resolveCardVariant(isLocked, isDone, isNew);

        return {
            ...quest,
            isDone,
            isLocked,
            isNew,
            currentStep,
            cardVariant
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

    private static convertStepRawToViewModel(stepRawNumber: string, stepRaw: StepRaw, steps: Step[]): StepViewModel {
        const step = steps.find(stepEntry => stepEntry.number.toString() === stepRawNumber)!;

        return {
            number: stepRawNumber,
            requisites: stepRaw.requisites.map(requisiteRaw => ({
                id: requisiteRaw.id,
                isDone: this.calculateIfRequisiteIsDone(requisiteRaw.id, step.requisites)
            })),
            isDone: this.calculateIfStepIsDone(stepRawNumber, steps),
            location: stepRaw.location,
            coordinates: {
                x: stepRaw.coordinates.x,
                y: stepRaw.coordinates.y
            },
            zoomedLocations: stepRaw.zoomedLocations.map(zoomedLocationRaw => ({
                location: zoomedLocationRaw.location,
                coordinates: {
                    x: zoomedLocationRaw.coordinates.x,
                    y: zoomedLocationRaw.coordinates.y
                }
            }))
        };
    }
}
