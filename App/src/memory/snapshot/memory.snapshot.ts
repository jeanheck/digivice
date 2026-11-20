import { MemoryReadError } from "@/memory/snapshot/memory-read.error";
import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";

export class MemorySnapshot {
  private readonly view: DataView;

  public constructor(ram: ArrayBuffer) {
    if (ram.byteLength !== PlayStationRamSize) {
      throw new RangeError(
        `Snapshot must be ${PlayStationRamSize} bytes (2 MiB PS1 RAM). Got ${ram.byteLength} bytes.`,
      );
    }

    this.view = new DataView(ram);
  }

  private ensureReadable(address: number, length: number): void {
    if (!Number.isInteger(address) || address < 0 || address + length > this.view.byteLength) {
      throw new MemoryReadError(
        address,
        `Failed to read ${length} byte(s) at 0x${address.toString(16).toUpperCase()}: outside the snapshot.`,
      );
    }
  }

  public readByte(address: number, bitMask?: number): number {
    this.ensureReadable(address, 1);
    const value = this.view.getUint8(address);

    if (bitMask === undefined) {
      return value;
    }

    return value & bitMask;
  }

  public readInt16(address: number): number {
    this.ensureReadable(address, 2);
    return this.view.getInt16(address, true);
  }

  public readInt32(address: number): number {
    this.ensureReadable(address, 4);
    return this.view.getInt32(address, true);
  }

  public readBytes(address: number, length: number): Uint8Array {
    this.ensureReadable(address, length);
    return new Uint8Array(this.view.buffer.slice(address, address + length));
  }
}
