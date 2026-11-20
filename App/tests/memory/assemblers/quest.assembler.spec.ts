import { describe, expect, it } from "vitest";
import { QuestAssembler } from "@/memory/assemblers/quest.assembler";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";

describe("QuestAssembler", () => {
  describe("assemble", () => {
    it("should map every requisite and step to isDone when the values are non-zero", () => {
      const resource: QuestResource = {
        id: "fishingPole",
        requisites: [{ id: "folderBag", value: 1 }],
        steps: [
          {
            number: 3,
            value: 0x80,
            requisites: [
              { id: "spiderWeb", value: 0xff },
              { id: "redSnapper", value: 0 },
            ],
          },
        ],
      };

      const quest = QuestAssembler.assemble(resource);

      expect(quest).toEqual({
        id: "fishingPole",
        requisites: [{ id: "folderBag", isDone: true }],
        steps: [
          {
            number: 3,
            isDone: true,
            requisites: [
              { id: "spiderWeb", isDone: true },
              { id: "redSnapper", isDone: false },
            ],
          },
        ],
      });
    });

    it.each([
      [0, false],
      [1, true],
      [0x80, true],
    ])("should map a requisite with value %i to isDone %s", (value, isDone) => {
      const quest = QuestAssembler.assemble({
        id: "quest",
        requisites: [{ id: "requisite", value }],
        steps: [],
      });

      expect(quest.requisites).toEqual([{ id: "requisite", isDone }]);
    });

    it.each([
      [0, false],
      [1, true],
      [0x80, true],
    ])("should map a step with value %i to isDone %s", (value, isDone) => {
      const quest = QuestAssembler.assemble({
        id: "quest",
        requisites: [],
        steps: [{ number: 1, value, requisites: [] }],
      });

      expect(quest.steps).toEqual([{ number: 1, isDone, requisites: [] }]);
    });

    it("should not cascade earlier steps when only a later step is done", () => {
      const quest = QuestAssembler.assemble({
        id: "treeBoots",
        requisites: [],
        steps: [
          { number: 1, value: 0, requisites: [] },
          { number: 2, value: 1, requisites: [] },
        ],
      });

      expect(quest.steps.map((step) => step.isDone)).toEqual([false, true]);
    });
  });
});
