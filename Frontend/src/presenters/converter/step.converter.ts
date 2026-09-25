import type { Step } from "@/models";
import { RequisiteConverter } from "@/presenters/converter/requisite.converter";
import type { InnerLocationRaw } from "@/repositories/tables/raws/location/inner-location.raw";
import type { StepRaw } from "@/repositories/tables/raws/quest/step.raw";
import { LocationService } from "@/services/location.service";
import type { InnerLocationViewModel } from "@/viewmodels/quest/inner-location.viewmodel";
import type { StepViewModel } from "@/viewmodels/quest/step.viewmodel";

export class StepConverter {
  public static convert(
    stepNumber: string,
    stepRaw: StepRaw,
    steps: Step[],
    partyLevel: number,
  ): StepViewModel {
    const step = steps.find((stepEntry) => {
      return stepEntry.number.toString() === stepNumber;
    })!;

    return {
      number: stepNumber,
      requisites: stepRaw.requisites.map((requisiteRaw) => {
        return RequisiteConverter.convert(requisiteRaw, step.requisites, partyLevel);
      }),
      isDone: steps.some((stepEntry) => {
        return stepEntry.number.toString() === stepNumber && stepEntry.isDone;
      }),
      location: stepRaw.location,
      coordinates: {
        x: stepRaw.coordinates.x,
        y: stepRaw.coordinates.y,
      },
      innerLocation: StepConverter.resolveInnerLocation(stepRaw).map((innerLocationRaw) => {
        return StepConverter.convertInnerLocation(innerLocationRaw);
      }),
    };
  }

  private static resolveInnerLocation(stepRaw: StepRaw): InnerLocationRaw[] {
    if (stepRaw.innerLocation !== undefined) {
      return stepRaw.innerLocation;
    }

    return LocationService.getInnerLocation(stepRaw.location);
  }

  private static convertInnerLocation(innerLocationRaw: InnerLocationRaw): InnerLocationViewModel {
    return {
      location: innerLocationRaw.location,
      coordinates: {
        x: innerLocationRaw.coordinates.x,
        y: innerLocationRaw.coordinates.y,
      },
    };
  }
}
