import type { Journal, Quest } from "@/state/models";
import type { MomentRaw } from "@/presentation/repositories/tables/raws/quest/moment.raw";

export class MomentService {
  public static isActive(moment: MomentRaw | undefined, journal: Journal): boolean {
    if (moment === undefined) {
      return true;
    }

    const quest = MomentService.findQuestById(journal, moment.quest);
    if (quest === null) {
      return false;
    }

    if (!MomentService.isStepDone(quest, moment.starts)) {
      return false;
    }

    if (moment.ends !== undefined && MomentService.isStepDone(quest, moment.ends)) {
      return false;
    }

    return true;
  }

  private static findQuestById(journal: Journal, questId: string): Quest | null {
    const quests = [
      journal.mainQuest,
      ...journal.sideQuests,
      ...journal.legendaryWeapons,
      ...journal.driAgents,
      ...journal.duelIsland,
    ];

    return quests.find((quest) => quest.id === questId) ?? null;
  }

  private static isStepDone(quest: Quest, stepNumber: number): boolean {
    return quest.steps.some((step) => step.number === stepNumber && step.isDone);
  }
}
