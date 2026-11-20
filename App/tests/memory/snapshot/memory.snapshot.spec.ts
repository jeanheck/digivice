import { describe, expect, it } from "vitest";
import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { MemorySnapshot } from "@/memory/snapshot/memory.snapshot";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "../../helpers/fake-snapshot.builder";

const LastAddress = PlayStationRamSize - 1;

describe("MemorySnapshot", () => {
  describe("constructor", () => {
    it("should throw when the buffer is not 2 MiB", () => {
      const shortRam = new FakeSnapshotBuilder(16).build();

      expect(() => new MemorySnapshot(shortRam)).toThrow(RangeError);
    });
  });

  describe("readByte", () => {
    it("should return the unsigned byte when no bit mask is provided", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeByte(0x800, 0x2a).build());

      expect(snapshot.readByte(0x800)).toBe(0x2a);
    });

    it("should return 255 when the byte is 0xFF", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeByte(0x800, 0xff).build());

      expect(snapshot.readByte(0x800)).toBe(0xff);
    });

    it("should apply the bit mask when a bit mask is provided", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeByte(0x800, 90).build());

      expect(snapshot.readByte(0x800, 15)).toBe(10);
    });

    it("should return zero when the masked bit is not set", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeByte(0x800, 0x7f).build());

      expect(snapshot.readByte(0x800, 0x80)).toBe(0);
    });

    it("should read the last byte of the snapshot", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeByte(LastAddress, 1).build(),
      );

      expect(snapshot.readByte(LastAddress)).toBe(1);
    });
  });

  describe("readInt16", () => {
    it("should return the little-endian value", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeInt16(0x800, 45).build());

      expect(snapshot.readInt16(0x800)).toBe(45);
    });

    it("should return -1 when the value is the 0xFFFF sentinel", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeInt16(0x800, 0xffff).build(),
      );

      expect(snapshot.readInt16(0x800)).toBe(-1);
    });

    it("should return negative values", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeInt16(0x800, -4).build());

      expect(snapshot.readInt16(0x800)).toBe(-4);
    });
  });

  describe("readInt32", () => {
    it("should return the little-endian value", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder().writeInt32(0x800, 9999).build(),
      );

      expect(snapshot.readInt32(0x800)).toBe(9999);
    });

    it("should return negative values", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeInt32(0x800, -1).build());

      expect(snapshot.readInt32(0x800)).toBe(-1);
    });
  });

  describe("readBytes", () => {
    it("should return the bytes in order", () => {
      const snapshot = new MemorySnapshot(
        new FakeSnapshotBuilder()
          .writeByte(0x800, 1)
          .writeByte(0x801, 2)
          .writeByte(0x802, 3)
          .build(),
      );

      expect([...snapshot.readBytes(0x800, 3)]).toEqual([1, 2, 3]);
    });

    it("should return a copy that does not share memory with the snapshot", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().writeByte(0x800, 1).build());

      snapshot.readBytes(0x800, 1)[0] = 0x7f;

      expect(snapshot.readByte(0x800)).toBe(1);
    });
  });

  describe("bounds", () => {
    it("should throw MemoryReadError with the address when reading past the end", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() => snapshot.readByte(PlayStationRamSize)).toThrow(MemoryReadError);
      expect(() => snapshot.readInt16(LastAddress)).toThrow(MemoryReadError);
      expect(() => snapshot.readInt32(PlayStationRamSize - 3)).toThrow(MemoryReadError);
      expect(() => snapshot.readBytes(LastAddress, 2)).toThrow(MemoryReadError);
    });

    it("should expose the failing address on MemoryReadError", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() => snapshot.readInt32(0x800100 * 4)).toThrow(
        expect.objectContaining({ name: "MemoryReadError", address: 0x800100 * 4 }),
      );
    });

    it("should throw MemoryReadError when the address is negative or not an integer", () => {
      const snapshot = new MemorySnapshot(new FakeSnapshotBuilder().build());

      expect(() => snapshot.readByte(-1)).toThrow(MemoryReadError);
      expect(() => snapshot.readInt16(1.5)).toThrow(MemoryReadError);
    });
  });
});
