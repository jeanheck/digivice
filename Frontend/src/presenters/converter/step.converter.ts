import type { Step } from "@/models";
import { RequisiteConverter } from "@/presenters/converter/requisite.converter";
import type { StepRaw } from "@/repositories/tables/raws/quest/step.raw";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

export class StepConverter {
    public static convert(stepNumber: string, stepRaw: StepRaw, steps: Step[]): StepViewModel {
        const step = steps.find((stepEntry) => {
            return stepEntry.number.toString() === stepNumber;
        })!;

        return {
            number: stepNumber,
            requisites: stepRaw.requisites.map((requisiteRaw) => {
                return RequisiteConverter.convert(requisiteRaw, step.requisites);
            }),
            isDone: steps.some((stepEntry) => {
                return stepEntry.number.toString() === stepNumber && stepEntry.isDone;
            }),
            location: stepRaw.location,
            coordinates: {
                x: stepRaw.coordinates.x,
                y: stepRaw.coordinates.y,
            },
            zoomedLocations: stepRaw.zoomedLocations.map((zoomedLocationRaw) => {
                return {
                    location: zoomedLocationRaw.location,
                    coordinates: {
                        x: zoomedLocationRaw.coordinates.x,
                        y: zoomedLocationRaw.coordinates.y,
                    },
                };
            }),
        };
    }
}
