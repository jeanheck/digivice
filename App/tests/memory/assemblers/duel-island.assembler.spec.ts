import { describe, expect, it } from "vitest";
import { DuelIslandAssembler } from "@/memory/assemblers/duel-island.assembler";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";

function duelIslandQuest(
  id: string,
  requisiteValues: number[],
  stepValues: number[],
): QuestResource {
  return {
    id,
    requisites: requisiteValues.map((value) => ({ id: "asukaTrophy", value })),
    steps: stepValues.map((value, index) => ({ number: index + 1, value, requisites: [] })),
  };
}

function doneFlags(resource: QuestResource): boolean[] {
  return DuelIslandAssembler.assemble(resource).steps.map((step) => step.isDone);
}

describe("DuelIslandAssembler", () => {
  describe("assemble", () => {
    it("should mark every step as done when the trophy is obtained", () => {
      const resource = duelIslandQuest("asukaTrophy", [], [0, 0, 0, 0, 0, 1]);

      expect(doneFlags(resource)).toEqual([true, true, true, true, true, true]);
    });

    it("should keep the raw progress when the trophy is not obtained", () => {
      const resource = duelIslandQuest("asukaTrophy", [], [0x80, 0x01, 0, 0, 0, 0]);

      expect(doneFlags(resource)).toEqual([true, true, false, false, false, false]);
    });

    it("should normalize the Asuka rematch when its last step is done", () => {
      const resource = duelIslandQuest("asukaRematch", [1], [0, 0, 0, 0, 0, 1]);

      expect(doneFlags(resource)).toEqual([true, true, true, true, true, true]);
    });

    it("should suppress every step when a requisite is not met", () => {
      const resource = duelIslandQuest("asukaRematch", [0], [0x80, 0x01, 0x02, 0x04, 0x08, 0]);

      expect(doneFlags(resource)).toEqual([false, false, false, false, false, false]);
    });

    it("should suppress every step even when the trophy step would cascade", () => {
      const resource = duelIslandQuest("asukaRematch", [0], [0, 0, 0, 0, 0, 1]);

      expect(doneFlags(resource)).toEqual([false, false, false, false, false, false]);
    });

    it("should keep the requisites as read when the steps are suppressed", () => {
      const quest = DuelIslandAssembler.assemble(duelIslandQuest("asukaRematch", [0], [1]));

      expect(quest.requisites).toEqual([{ id: "asukaTrophy", isDone: false }]);
    });

    it("should return a quest without steps when there are no steps", () => {
      const quest = DuelIslandAssembler.assemble(duelIslandQuest("asukaRematch", [0], []));

      expect(quest).toEqual({
        id: "asukaRematch",
        requisites: [{ id: "asukaTrophy", isDone: false }],
        steps: [],
      });
    });

    it("should not mutate the resource", () => {
      const resource = duelIslandQuest("asukaTrophy", [], [0, 1]);
      const originalResource = structuredClone(resource);

      DuelIslandAssembler.assemble(resource);

      expect(resource).toEqual(originalResource);
    });
  });
});
