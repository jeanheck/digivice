import type { Quest } from "@/models";
import type { QuestDTO } from "@/events/dto/journals/quest.dto";
import { RequisiteSyncer } from "./quests/requisite.syncer";
import { StepSyncer } from "./quests/step.syncer";

export class QuestSyncer {
  public static sync(previousQuest: Quest, newQuestDto: QuestDTO): void {
    if (newQuestDto.requisites) {
      newQuestDto.requisites.forEach((newRequisiteDto) => {
        const previousRequisite = previousQuest.requisites.find(
          (requisite) => requisite.id === newRequisiteDto.id,
        );

        if (previousRequisite) {
          RequisiteSyncer.sync(previousRequisite, newRequisiteDto);
        }
      });
    }

    if (newQuestDto.steps) {
      newQuestDto.steps.forEach((newStepDto) => {
        const previousStep = previousQuest.steps.find((step) => step.number === newStepDto.number);

        if (previousStep) {
          StepSyncer.sync(previousStep, newStepDto);
        }
      });
    }
  }
}
