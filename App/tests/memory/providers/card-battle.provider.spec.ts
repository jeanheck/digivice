import { describe, expect, it } from "vitest";
import { CardBattleProvider } from "@/memory/providers/card-battle.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("CardBattleProvider", () => {
  describe("get", () => {
    it("should return a null id when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const cardBattle = CardBattleProvider.get(snapshot);

      expect(cardBattle).toEqual({ id: null });
    });

    it("should return the id when the real card battle address holds a positive value", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeInt32(0x0004b404, 7).build(),
      );

      const cardBattle = CardBattleProvider.get(snapshot);

      expect(cardBattle).toEqual({ id: 7 });
    });

    it("should return a null id when the real card battle address holds zero", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      const cardBattle = CardBattleProvider.get(snapshot);

      expect(cardBattle).toEqual({ id: null });
    });
  });
});
