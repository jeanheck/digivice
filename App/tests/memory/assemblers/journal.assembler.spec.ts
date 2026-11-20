import { describe, expect, it } from "vitest";
import type { Quest } from "@/state/models";
import { JournalAssembler } from "@/memory/assemblers/journal.assembler";
import type { JournalResource } from "@/memory/resources/journal.resource";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";

function questWithOnlyLastStepDone(id: string): QuestResource {
  return {
    id,
    requisites: [],
    steps: [
      { number: 1, value: 0, requisites: [] },
      { number: 2, value: 0, requisites: [] },
      { number: 3, value: 1, requisites: [] },
    ],
  };
}

function emptyJournal(): JournalResource {
  return {
    mainQuest: { id: "mainQuest", requisites: [], steps: [] },
    sideQuests: [],
    legendaryWeapons: [],
    driAgents: [],
    duelIsland: [],
  };
}

function doneFlags(quest: Quest | undefined): boolean[] | undefined {
  return quest?.steps.map((step) => step.isDone);
}

describe("JournalAssembler", () => {
  describe("assemble", () => {
    it("should apply the completion cascade to the main quest", () => {
      const resource = { ...emptyJournal(), mainQuest: questWithOnlyLastStepDone("mainQuest") };

      const journal = JournalAssembler.assemble(resource);

      expect(doneFlags(journal.mainQuest)).toEqual([true, true, true]);
    });

    it("should not apply the completion cascade to side quests", () => {
      const resource = { ...emptyJournal(), sideQuests: [questWithOnlyLastStepDone("treeBoots")] };

      const journal = JournalAssembler.assemble(resource);

      expect(doneFlags(journal.sideQuests[0])).toEqual([false, false, true]);
    });

    it("should not apply the completion cascade to legendary weapons", () => {
      const resource = {
        ...emptyJournal(),
        legendaryWeapons: [questWithOnlyLastStepDone("eternally")],
      };

      const journal = JournalAssembler.assemble(resource);

      expect(doneFlags(journal.legendaryWeapons[0])).toEqual([false, false, true]);
    });

    it("should not apply the completion cascade to DRI agents", () => {
      const resource = {
        ...emptyJournal(),
        driAgents: [questWithOnlyLastStepDone("driAgentGuilmon")],
      };

      const journal = JournalAssembler.assemble(resource);

      expect(doneFlags(journal.driAgents[0])).toEqual([false, false, true]);
    });

    it("should apply the Duel Island rules to Duel Island", () => {
      const suppressedQuest: QuestResource = {
        id: "asukaRematch",
        requisites: [{ id: "asukaTrophy", value: 0 }],
        steps: [
          { number: 1, value: 0x80, requisites: [] },
          { number: 2, value: 0, requisites: [] },
        ],
      };
      const resource = {
        ...emptyJournal(),
        duelIsland: [questWithOnlyLastStepDone("asukaTrophy"), suppressedQuest],
      };

      const journal = JournalAssembler.assemble(resource);

      expect(journal.duelIsland).toHaveLength(2);
      expect(doneFlags(journal.duelIsland[0])).toEqual([true, true, true]);
      expect(doneFlags(journal.duelIsland[1])).toEqual([false, false]);
    });

    it("should keep the order of every category", () => {
      const resource = {
        ...emptyJournal(),
        sideQuests: [questWithOnlyLastStepDone("first"), questWithOnlyLastStepDone("second")],
      };

      const journal = JournalAssembler.assemble(resource);

      expect(journal.sideQuests.map((quest) => quest.id)).toEqual(["first", "second"]);
    });
  });
});
