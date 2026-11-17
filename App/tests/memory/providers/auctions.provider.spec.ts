import { describe, expect, it } from "vitest";
import { AuctionsProvider } from "@/memory/providers/auctions.provider";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";
import { SnapshotFixtureLoader } from "../../helpers/snapshot-fixture.loader";

describe("AuctionsProvider", () => {
  describe("get", () => {
    it("should return the auctions when the snapshot is the Central Park fixture", () => {
      const snapshot = new MemorySnapshot(SnapshotFixtureLoader.load("field-central-park"));

      const auctions = AuctionsProvider.get(snapshot);

      expect(auctions).toEqual({
        divineBarrier: true,
        hazardShield: true,
        sniperShield: false,
        dramonShield: true,
        yinYangWand: false,
        hazardFang: false,
        sniperCannon: false,
        roninBlade: false,
        sniperGoggle: false,
        divineCrown: false,
        braveHelmet: false,
      });
    });

    it("should read the real auctions addresses when the snapshot is synthetic", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(0x0004b38a, 0xff).writeByte(0x0004b38b, 0x07).build(),
      );

      const auctions = AuctionsProvider.get(snapshot);

      expect(Object.values(auctions).every((consumed) => consumed === true)).toBe(true);
    });
  });
});
