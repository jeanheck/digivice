import { describe, expect, it } from "vitest";
import { JournalProvider } from "@/memory/providers/journal.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { BattleAgainstNatsumiExpected } from "../../fixtures/snapshots/battle-against-natsumi.expected";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("JournalProvider", () => {
  describe("get", () => {
    it("should return the journal when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const journal = JournalProvider.get(snapshot);

      expect(journal).toEqual(FieldCentralParkExpected.journal);
    });

    it("should return the journal when the snapshot is the battle against Natsumi fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("battle-against-natsumi"));

      const journal = JournalProvider.get(snapshot);

      expect(journal).toEqual(BattleAgainstNatsumiExpected.journal);
    });

    it("should return every quest not done when the RAM is empty", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      const journal = JournalProvider.get(snapshot);

      const quests = [
        journal.mainQuest,
        ...journal.sideQuests,
        ...journal.legendaryWeapons,
        ...journal.driAgents,
        ...journal.duelIsland,
      ];
      expect(quests).toHaveLength(19);
      quests.forEach((quest) => {
        expect(quest.requisites.every((requisite) => !requisite.isDone)).toBe(true);
        expect(quest.steps.every((step) => !step.isDone)).toBe(true);
      });
    });
  });
});
