import type { Journal, Quest, Requisite, Step } from "@/models";
import { QuestRepository } from "@/repositories/quest.repository";
import type { QuestRaw } from "@/repositories/tables/raws/quest/quest.raw";
import type { StepRaw } from "@/repositories/tables/raws/quest/step.raw";
import type { JournalViewModel } from "@/viewmodels/quest/journal.viewmodel";
import type { QuestViewModel } from "@/viewmodels/quest/quest.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

export class JournalPresenter {
    public static getJournalViewModel(journal: Journal): JournalViewModel {
        const mainQuestRaw = QuestRepository.getMainQuestRaw();
        const mainQuestViewModel = this.convertQuestRawToViewModel(mainQuestRaw, journal.mainQuest!);

        const sideQuestsRaw = QuestRepository.getSideQuestsRaw();
        const sideQuestsViewModels = sideQuestsRaw
            .map(sideQuestRaw => this.convertQuestRawToViewModel(sideQuestRaw, this.getSideQuestFromJournal(journal, sideQuestRaw.id)));

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
    private static convertQuestRawToViewModel(questRaw: QuestRaw, quest: Quest): QuestViewModel {
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
    private static convertStepRawToViewModel(stepRawNumber: string, stepRaw: StepRaw, steps: Step[]): StepViewModel {
        const step = steps.find(s => s.number.toString() === stepRawNumber)!;

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
