import { describe, expect, it } from "vitest";
import type { QuestAddresses } from "@/memory/addresses/journals/quest.addresses";
import type { RequisiteAddresses } from "@/memory/addresses/journals/quests/requisite.addresses";
import type { StepAddresses } from "@/memory/addresses/journals/quests/step.addresses";
import { QuestReader } from "@/memory/readers/quest.reader";
import { AddressesRepository } from "@/memory/repositories/addresses.repository";
import type { QuestResource } from "@/memory/resources/journals/quest.resource";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

function questWithRequisite(requisite: RequisiteAddresses): QuestAddresses {
  return { id: "quest", requisites: [requisite], steps: [] };
}

function questWithStep(step: StepAddresses): QuestAddresses {
  return { id: "quest", requisites: [], steps: [step] };
}

function snapshotWithBytes(bytes: [address: number, value: number][]): MemorySnapshot {
  const builder = new FakeSnapshotBuilder();
  bytes.forEach(([address, value]) => builder.writeByte(address, value));
  return new MemorySnapshot(builder.build());
}

function findQuest(quests: QuestResource[], id: string): QuestResource {
  const quest = quests.find((questResource) => questResource.id === id);
  if (quest === undefined) {
    throw new Error(`Quest ${id} not found.`);
  }

  return quest;
}

function stepValues(quest: QuestResource): number[] {
  return quest.steps.map((step) => step.value);
}

describe("QuestReader", () => {
  describe("read requisites", () => {
    it("should map the requisite resource when the requisite has no bit mask", () => {
      const snapshot = snapshotWithBytes([[0x7500, 7]]);

      const resource = QuestReader.read(
        snapshot,
        questWithRequisite({ id: "ReqX", address: 0x7500, bitMasks: [] }),
      );

      expect(resource.requisites).toEqual([{ id: "ReqX", value: 7 }]);
    });

    it("should apply the bit mask when a single bit mask is provided", () => {
      const snapshot = snapshotWithBytes([[0x4b3e5, 0xff]]);

      const resource = QuestReader.read(
        snapshot,
        questWithRequisite({ id: "byakkoLeader", address: 0x4b3e5, bitMasks: [0x40] }),
      );

      expect(resource.requisites[0]?.value).toBe(0x40);
    });

    it("should return zero when not all bit masks are set", () => {
      const snapshot = snapshotWithBytes([[0x4b370, 0x01]]);

      const resource = QuestReader.read(
        snapshot,
        questWithRequisite({ id: "ReqMulti", address: 0x4b370, bitMasks: [0x01, 0x08] }),
      );

      expect(resource.requisites[0]?.value).toBe(0);
    });

    it("should return the first bit mask value when all bit masks are set", () => {
      const snapshot = snapshotWithBytes([[0x4b370, 0x09]]);

      const resource = QuestReader.read(
        snapshot,
        questWithRequisite({ id: "ReqMulti", address: 0x4b370, bitMasks: [0x01, 0x08] }),
      );

      expect(resource.requisites[0]?.value).toBe(0x01);
    });
  });

  describe("read steps", () => {
    it("should map the step resource with its requisites when the addresses are synthetic", () => {
      const snapshot = snapshotWithBytes([
        [0x5500, 0x12],
        [0x6000, 5],
      ]);

      const resource = QuestReader.read(
        snapshot,
        questWithStep({
          number: 3,
          address: 0x5500,
          bitMasks: [0x0f],
          requisites: [{ id: "Req2", address: 0x6000, bitMasks: [] }],
        }),
      );

      expect(resource.steps).toEqual([
        { number: 3, value: 0x02, requisites: [{ id: "Req2", value: 5 }] },
      ]);
    });

    it("should return the raw byte when the step has no bit mask", () => {
      const snapshot = snapshotWithBytes([[0x48f42, 1]]);

      const resource = QuestReader.read(
        snapshot,
        questWithStep({ number: 1, address: 0x48f42, bitMasks: [], requisites: [] }),
      );

      expect(resource.steps[0]?.value).toBe(1);
    });

    it("should return zero when not all step bit masks are set", () => {
      const snapshot = snapshotWithBytes([[0x4b370, 0x01]]);

      const resource = QuestReader.read(
        snapshot,
        questWithStep({ number: 21, address: 0x4b370, bitMasks: [0x01, 0x08], requisites: [] }),
      );

      expect(resource.steps[0]?.value).toBe(0);
    });

    it("should return the first bit mask value when all step bit masks are set", () => {
      const snapshot = snapshotWithBytes([[0x4b370, 0x09]]);

      const resource = QuestReader.read(
        snapshot,
        questWithStep({ number: 21, address: 0x4b370, bitMasks: [0x01, 0x08], requisites: [] }),
      );

      expect(resource.steps[0]?.value).toBe(0x01);
    });

    it("should ignore bits outside every mask when all step bit masks are set", () => {
      const snapshot = snapshotWithBytes([[0x4b370, 0xf9]]);

      const resource = QuestReader.read(
        snapshot,
        questWithStep({ number: 21, address: 0x4b370, bitMasks: [0x01, 0x08], requisites: [] }),
      );

      expect(resource.steps[0]?.value).toBe(0x01);
    });
  });

  describe("read", () => {
    it("should keep the quest id, requisites and steps in definition order", () => {
      const snapshot = snapshotWithBytes([
        [0x1000, 1],
        [0x2000, 0x80],
        [0x2001, 0x01],
      ]);

      const resource = QuestReader.read(snapshot, {
        id: "treeBoots",
        requisites: [{ id: "folderBag", address: 0x1000, bitMasks: [] }],
        steps: [
          { number: 1, address: 0x2000, bitMasks: [0x80], requisites: [] },
          { number: 2, address: 0x2001, bitMasks: [0x01], requisites: [] },
        ],
      });

      expect(resource).toEqual({
        id: "treeBoots",
        requisites: [{ id: "folderBag", value: 1 }],
        steps: [
          { number: 1, value: 0x80, requisites: [] },
          { number: 2, value: 0x01, requisites: [] },
        ],
      });
    });

    it("should return empty lists when the quest has no requisites and no steps", () => {
      const snapshot = snapshotWithBytes([]);

      const resource = QuestReader.read(snapshot, { id: "empty", requisites: [], steps: [] });

      expect(resource).toEqual({ id: "empty", requisites: [], steps: [] });
    });

    it("should throw MemoryReadError when a step requisite address is outside the snapshot", () => {
      const snapshot = snapshotWithBytes([]);

      expect(() =>
        QuestReader.read(
          snapshot,
          questWithStep({
            number: 1,
            address: 0x1000,
            bitMasks: [],
            requisites: [{ id: "outside", address: PlayStationRamSize, bitMasks: [] }],
          }),
        ),
      ).toThrow(MemoryReadError);
    });
  });

  describe("read with the real definitions", () => {
    it("should read the main quest", () => {
      const snapshot = snapshotWithBytes([
        [0x0004b3b6, 0x80],
        [0x0004b3b7, 0x01],
      ]);

      const mainQuest = QuestReader.read(snapshot, AddressesRepository.getMainQuest());

      expect(mainQuest.id).toBe("mainQuest");
      expect(mainQuest.requisites).toEqual([]);
      expect(mainQuest.steps).toHaveLength(99);
      expect(mainQuest.steps[0]).toMatchObject({ number: 1, value: 0x80 });
      expect(mainQuest.steps[1]).toMatchObject({ number: 2, value: 0x01 });
      expect(mainQuest.steps.at(-1)?.number).toBe(99);
    });

    it("should read the side quests", () => {
      const snapshot = snapshotWithBytes([
        [0x00048f42, 1],
        [0x0004b3b0, 0xa4],
        [0x00048e57, 1],
        [0x00048e09, 1],
        [0x00048db7, 0],
        [0x00048db5, 1],
      ]);

      const sideQuests = AddressesRepository.getAllSideQuests().map((addresses) =>
        QuestReader.read(snapshot, addresses),
      );

      expect(sideQuests).toHaveLength(3);

      const folderBag = findQuest(sideQuests, "folderBag");
      expect(folderBag.requisites).toEqual([]);
      expect(stepValues(folderBag)).toEqual([1]);

      const treeBoots = findQuest(sideQuests, "treeBoots");
      expect(treeBoots.requisites).toEqual([{ id: "folderBag", value: 1 }]);
      expect(treeBoots.steps).toHaveLength(7);
      expect(treeBoots.steps[0]?.value).toBe(0x04);
      expect(treeBoots.steps[3]?.value).toBe(0xa0);
      expect(treeBoots.steps[4]?.value).toBe(0x80);

      const fishingPole = findQuest(sideQuests, "fishingPole");
      expect(fishingPole.requisites).toHaveLength(1);
      expect(fishingPole.steps).toHaveLength(3);
      expect(fishingPole.steps[2]).toEqual({
        number: 3,
        value: 1,
        requisites: [
          { id: "bambooSpear", value: 1 },
          { id: "spiderWeb", value: 1 },
          { id: "redSnapper", value: 0 },
        ],
      });
    });

    it("should read the legendary weapons", () => {
      const snapshot = snapshotWithBytes([[0x0004b38e, 0x1f]]);

      const legendaryWeapons = AddressesRepository.getAllLegendaryWeapons().map((addresses) =>
        QuestReader.read(snapshot, addresses),
      );

      expect(legendaryWeapons).toHaveLength(5);
      expect(findQuest(legendaryWeapons, "eternally")).toEqual({
        id: "eternally",
        requisites: [],
        steps: [{ number: 1, value: 0x01, requisites: [] }],
      });

      const weaponsWithRequisite: [id: string, requisiteId: string, stepValue: number][] = [
        ["invincible", "seiryuLeader", 0x02],
        ["muramasa", "smellHerb", 0x04],
        ["superNova", "submarimon", 0x08],
        ["punishment", "numemon", 0x10],
      ];
      weaponsWithRequisite.forEach(([id, requisiteId, stepValue]) => {
        expect(findQuest(legendaryWeapons, id)).toEqual({
          id,
          requisites: [{ id: requisiteId, value: 0 }],
          steps: [{ number: 1, value: stepValue, requisites: [] }],
        });
      });
    });

    it("should read the DRI agents", () => {
      const snapshot = snapshotWithBytes([
        [0x0004b38c, 0xff],
        [0x0004b3b7, 0xfc],
        [0x0004b3b8, 0x03],
        [0x00048dd2, 1],
        [0x0004a7e0, 0x08],
        [0x00048db6, 1],
        [0x0004a028, 0x06],
        [0x00048dd3, 1],
        [0x0004a404, 0x07],
        [0x00048f3b, 1],
        [0x00049870, 0x04],
        [0x00048f18, 1],
        [0x00049c4c, 0x05],
        [0x00048dc3, 1],
        [0x00049494, 0x03],
        [0x00048dd6, 1],
        [0x0004abbc, 0x09],
        [0x00048dd7, 1],
        [0x0004af98, 0x0a],
      ]);

      const driAgents = AddressesRepository.getAllDriAgents().map((addresses) =>
        QuestReader.read(snapshot, addresses),
      );

      expect(driAgents).toHaveLength(8);

      const expectedAgents: [
        id: string,
        requisiteId: string,
        stepValues: number[],
        dDnaId: string,
      ][] = [
        ["driAgentGuilmon", "seiryuLeader", [0x02, 0x08, 0x08], "guilmonDDNA"],
        ["driAgentAgumon", "seiryuLeader", [0x01, 0x04, 0x06], "agumonDDNA"],
        ["driAgentVeemon", "byakkoLeader", [0x80, 0x02, 0x07], "veemonDDNA"],
        ["driAgentKumamon", "submarimon", [0x20, 0x01, 0x04], "kumamonDDNA"],
        ["driAgentMonmon", "submarimon", [0x40, 0x80, 0x05], "monmonDDNA"],
        ["driAgentKotemon", "submarimon", [0x10, 0x40, 0x03], "kotemonDDNA"],
        ["driAgentRenamon", "submarimon", [0x08, 0x20, 0x09], "renamonDDNA"],
        ["driAgentPatamon", "submarimon", [0x04, 0x10, 0x0a], "patamonDDNA"],
      ];
      expectedAgents.forEach(([id, requisiteId, expectedStepValues, dDnaId]) => {
        const agent = findQuest(driAgents, id);
        expect(agent.requisites).toEqual([{ id: requisiteId, value: 0 }]);
        expect(stepValues(agent)).toEqual(expectedStepValues);
        expect(agent.steps[2]?.requisites).toEqual([{ id: dDnaId, value: 1 }]);
      });
    });

    it("should read the Duel Island quests", () => {
      const snapshot = snapshotWithBytes([
        [0x0004b3b2, 0xcf],
        [0x0004b3b3, 0x0f],
        [0x00048dc2, 0x01],
        [0x00048dc0, 0x03],
      ]);

      const duelIsland = AddressesRepository.getAllDuelIsland().map((addresses) =>
        QuestReader.read(snapshot, addresses),
      );

      expect(duelIsland).toHaveLength(2);

      const asukaTrophy = findQuest(duelIsland, "asukaTrophy");
      expect(asukaTrophy.requisites).toEqual([{ id: "submarimon", value: 0x01 }]);
      expect(stepValues(asukaTrophy)).toEqual([0x80, 0x01, 0x02, 0x04, 0x08, 1]);

      const asukaRematch = findQuest(duelIsland, "asukaRematch");
      expect(asukaRematch.requisites).toEqual([{ id: "asukaTrophy", value: 1 }]);
      expect(stepValues(asukaRematch)).toEqual([0x80, 0x01, 0x02, 0x04, 0x08, 0x08]);
    });
  });
});
