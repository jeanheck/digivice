import type { Quest } from "@/state/models";
import { QuestAssembler } from "@/memory/assemblers/quest.assembler";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";

export class MainQuestAssembler {
  private static normalize(mainQuest: Quest): Quest {
    const lastDoneStepIndex = mainQuest.steps.findLastIndex((step) => step.isDone);

    // Completion cascade: a done step forces every earlier step to be done.
    return {
      ...mainQuest,
      steps: mainQuest.steps.map((step, index) =>
        index < lastDoneStepIndex ? { ...step, isDone: true } : step,
      ),
    };
  }

  public static assemble(resource: QuestResource): Quest {
    const mainQuest = QuestAssembler.assemble(resource);
    return MainQuestAssembler.normalize(mainQuest);
  }
}
