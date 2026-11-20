import { describe, expect, it } from "vitest";
import { DigimonBattleProvider } from "@/memory/providers/digimon-battle.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { BattleAgainstNatsumiExpected } from "../../fixtures/snapshots/battle-against-natsumi.expected";
import { FieldCentralParkExpected } from "../../fixtures/snapshots/field-central-park.expected";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("DigimonBattleProvider", () => {
  describe("get", () => {
    it("should return Vegiemon as the enemy when the snapshot is the battle against Natsumi fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("battle-against-natsumi"));

      const digimonBattle = DigimonBattleProvider.get(snapshot);

      expect(digimonBattle).toEqual(BattleAgainstNatsumiExpected.digimonBattle);
    });

    it("should return a null enemy when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const digimonBattle = DigimonBattleProvider.get(snapshot);

      expect(digimonBattle).toEqual(FieldCentralParkExpected.digimonBattle);
    });
  });
});
