import type { Journal } from "@/state/models";
import { DuelIslandAssembler } from "@/memory/assemblers/duel-island.assembler";
import { MainQuestAssembler } from "@/memory/assemblers/main-quest.assembler";
import { QuestAssembler } from "@/memory/assemblers/quest.assembler";
import type { JournalResource } from "@/memory/resources/journal.resource";

export class JournalAssembler {
  public static assemble(resource: JournalResource): Journal {
    return {
      mainQuest: MainQuestAssembler.assemble(resource.mainQuest),
      sideQuests: resource.sideQuests.map((quest) => QuestAssembler.assemble(quest)),
      legendaryWeapons: resource.legendaryWeapons.map((quest) => QuestAssembler.assemble(quest)),
      driAgents: resource.driAgents.map((quest) => QuestAssembler.assemble(quest)),
      duelIsland: resource.duelIsland.map((quest) => DuelIslandAssembler.assemble(quest)),
    };
  }
}
