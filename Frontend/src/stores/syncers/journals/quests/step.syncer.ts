import type { Step } from "@/models";
import type { StepDTO } from "@/events/dto/journals/quests/step.dto";
import { RequisiteSyncer } from "./requisite.syncer";

export class StepSyncer {
  public static sync(previousStep: Step, newStepDto: StepDTO): void {
    if (newStepDto.isDone !== undefined) {
      previousStep.isDone = newStepDto.isDone;
    }

    if (newStepDto.requisites) {
      newStepDto.requisites.forEach((newRequisiteDto) => {
        const previousRequisite = previousStep.requisites.find(
          (requisite) => requisite.id === newRequisiteDto.id,
        );

        if (previousRequisite) {
          RequisiteSyncer.sync(previousRequisite, newRequisiteDto);
        }
      });
    }
  }
}
