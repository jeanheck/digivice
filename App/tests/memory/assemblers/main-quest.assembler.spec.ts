import { describe, expect, it } from "vitest";
import { MainQuestAssembler } from "@/memory/assemblers/main-quest.assembler";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";

function mainQuest(...stepValues: number[]): QuestResource {
  return {
    id: "mainQuest",
    requisites: [],
    steps: stepValues.map((value, index) => ({ number: index + 1, value, requisites: [] })),
  };
}

function doneFlags(resource: QuestResource): boolean[] {
  return MainQuestAssembler.assemble(resource).steps.map((step) => step.isDone);
}

describe("MainQuestAssembler", () => {
  describe("assemble", () => {
    it("should apply the completion cascade when a later step is done", () => {
      expect(doneFlags(mainQuest(0, 0, 1, 0))).toEqual([true, true, true, false]);
    });

    it("should cascade from the last step when only the last step is done", () => {
      expect(doneFlags(mainQuest(0, 0, 0, 0x80))).toEqual([true, true, true, true]);
    });

    it("should keep every step not done when no step is done", () => {
      expect(doneFlags(mainQuest(0, 0, 0))).toEqual([false, false, false]);
    });

    it("should keep every step done when every step is done", () => {
      expect(doneFlags(mainQuest(1, 1, 1))).toEqual([true, true, true]);
    });

    it("should handle a main quest without steps", () => {
      const quest = MainQuestAssembler.assemble(mainQuest());

      expect(quest).toEqual({ id: "mainQuest", requisites: [], steps: [] });
    });

    it("should fill every gap before the last done step", () => {
      expect(doneFlags(mainQuest(1, 0, 1, 0, 0, 1, 0))).toEqual([
        true,
        true,
        true,
        true,
        true,
        true,
        false,
      ]);
    });

    it("should keep the step requisites when the cascade marks a step as done", () => {
      const resource = mainQuest(0, 1);
      resource.steps[0]?.requisites.push({ id: "tntBall", value: 0 });

      const quest = MainQuestAssembler.assemble(resource);

      expect(quest.steps[0]).toEqual({
        number: 1,
        isDone: true,
        requisites: [{ id: "tntBall", isDone: false }],
      });
    });

    it("should not mutate the resource", () => {
      const resource = mainQuest(0, 1);
      const originalResource = structuredClone(resource);

      MainQuestAssembler.assemble(resource);

      expect(resource).toEqual(originalResource);
    });
  });
});
