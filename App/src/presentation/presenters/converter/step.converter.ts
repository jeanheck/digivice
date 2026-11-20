import type { Step } from "@/state/models";
import { RequisiteConverter } from "@/presentation/presenters/converter/requisite.converter";
import type { InnerLocationRaw } from "@/presentation/repositories/tables/raws/map/inner-location.raw";
import type { StepRaw } from "@/presentation/repositories/tables/raws/quest/step.raw";
import { MapService } from "@/presentation/services/map.service";
import type { InnerLocationViewModel } from "@/presentation/viewmodels/quest/inner-location.viewmodel";
import type { StepViewModel } from "@/presentation/viewmodels/quest/step.viewmodel";

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

    return MapService.getInnerLocation(stepRaw.location);
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
