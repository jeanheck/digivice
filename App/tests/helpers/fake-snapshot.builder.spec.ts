import { describe, expect, it } from "vitest";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";
import { FakeSnapshotBuilder } from "./fake-snapshot.builder";

describe("FakeSnapshotBuilder", () => {
  describe("build", () => {
    it("should return a zeroed 2 MiB snapshot by default", () => {
      const snapshot = new FakeSnapshotBuilder().build();

      expect(snapshot.byteLength).toBe(PlayStationRamSize);
      expect(new Uint8Array(snapshot).every((byte) => byte === 0)).toBe(true);
    });

    it("should honor a custom size", () => {
      const snapshot = new FakeSnapshotBuilder(16).build();

      expect(snapshot.byteLength).toBe(16);
    });

    it("should return a copy that is not affected by later writes", () => {
      const builder = new FakeSnapshotBuilder(4);
      const snapshot = builder.build();

      builder.writeByte(0, 0x7f);

      expect(new DataView(snapshot).getUint8(0)).toBe(0);
    });
  });

  describe("writeByte", () => {
    it("should write unsigned values", () => {
      const view = new DataView(new FakeSnapshotBuilder(4).writeByte(1, 0xff).build());

      expect(view.getUint8(1)).toBe(0xff);
    });

    it("should write negative values as two's complement", () => {
      const view = new DataView(new FakeSnapshotBuilder(4).writeByte(0, -1).build());

      expect(view.getInt8(0)).toBe(-1);
      expect(view.getUint8(0)).toBe(0xff);
    });
  });

  describe("writeInt16", () => {
    it("should write little-endian bytes", () => {
      const bytes = new Uint8Array(new FakeSnapshotBuilder(4).writeInt16(0, 0x0600).build());

      expect([bytes[0], bytes[1]]).toEqual([0x00, 0x06]);
    });

    it("should round-trip the 0xFFFF sentinel as -1 signed and 65535 unsigned", () => {
      const view = new DataView(new FakeSnapshotBuilder(4).writeInt16(0, -1).build());

      expect(view.getInt16(0, true)).toBe(-1);
      expect(view.getUint16(0, true)).toBe(0xffff);
    });

    it("should accept unsigned values up to 0xFFFF", () => {
      const view = new DataView(new FakeSnapshotBuilder(4).writeInt16(0, 0xffff).build());

      expect(view.getInt16(0, true)).toBe(-1);
    });
  });

  describe("writeInt32", () => {
    it("should write little-endian bytes", () => {
      const bytes = new Uint8Array(new FakeSnapshotBuilder(4).writeInt32(0, 0x12345678).build());

      expect([...bytes]).toEqual([0x78, 0x56, 0x34, 0x12]);
    });

    it("should round-trip negative values", () => {
      const view = new DataView(new FakeSnapshotBuilder(4).writeInt32(0, -4).build());

      expect(view.getInt32(0, true)).toBe(-4);
    });
  });

  describe("bounds", () => {
    it("should throw when writing past the end of the snapshot", () => {
      const builder = new FakeSnapshotBuilder(4);

      expect(() => builder.writeInt32(1, 0)).toThrow(RangeError);
      expect(() => builder.writeInt16(3, 0)).toThrow(RangeError);
      expect(() => builder.writeByte(4, 0)).toThrow(RangeError);
    });

    it("should throw when the address is negative or not an integer", () => {
      const builder = new FakeSnapshotBuilder(4);

      expect(() => builder.writeByte(-1, 0)).toThrow(RangeError);
      expect(() => builder.writeByte(1.5, 0)).toThrow(RangeError);
    });

    it("should throw when the value does not fit in the target size", () => {
      const builder = new FakeSnapshotBuilder(4);

      expect(() => builder.writeByte(0, 0x100)).toThrow(RangeError);
      expect(() => builder.writeByte(0, -0x81)).toThrow(RangeError);
      expect(() => builder.writeInt16(0, 0x10000)).toThrow(RangeError);
      expect(() => builder.writeInt16(0, -0x8001)).toThrow(RangeError);
      expect(() => builder.writeInt32(0, 0x100000000)).toThrow(RangeError);
      expect(() => builder.writeInt32(0, -0x80000001)).toThrow(RangeError);
      expect(() => builder.writeInt16(0, 1.5)).toThrow(RangeError);
    });
  });
});
