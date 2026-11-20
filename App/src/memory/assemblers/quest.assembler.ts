import type { Quest, Requisite, Step } from "@/state/models";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";
import type { RequisiteResource } from "@/memory/resources/journals/quests/requisite.resource";
import type { StepResource } from "@/memory/resources/journals/quests/step.resource";

export class QuestAssembler {
  private static assembleRequisite(resource: RequisiteResource): Requisite {
    return {
      id: resource.id,
      isDone: resource.value !== 0,
    };
  }

  private static assembleStep(resource: StepResource): Step {
    return {
      number: resource.number,
      isDone: resource.value !== 0,
      requisites: resource.requisites.map((requisite) =>
        QuestAssembler.assembleRequisite(requisite),
      ),
    };
  }

  public static assemble(resource: QuestResource): Quest {
    return {
      id: resource.id,
      requisites: resource.requisites.map((requisite) =>
        QuestAssembler.assembleRequisite(requisite),
      ),
      steps: resource.steps.map((step) => QuestAssembler.assembleStep(step)),
    };
  }
}
