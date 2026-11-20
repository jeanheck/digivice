import type { Quest } from "@/state/models";
import { QuestAssembler } from "@/memory/assemblers/quest.assembler";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";

export class DuelIslandAssembler {
  private static withAllSteps(quest: Quest, isDone: boolean): Quest {
    return {
      ...quest,
      steps: quest.steps.map((step) => ({ ...step, isDone })),
    };
  }

  private static normalize(quest: Quest): Quest {
    if (!quest.requisites.every((requisite) => requisite.isDone)) {
      return DuelIslandAssembler.withAllSteps(quest, false);
    }

    const trophyStep = quest.steps.at(-1);
    if (trophyStep === undefined || !trophyStep.isDone) {
      return quest;
    }

    return DuelIslandAssembler.withAllSteps(quest, true);
  }

  public static assemble(resource: QuestResource): Quest {
    const quest = QuestAssembler.assemble(resource);
    return DuelIslandAssembler.normalize(quest);
  }
}
