import { PlayStationRamSize } from "@/memory/snapshot/snapshot.constant";

export class FakeSnapshotBuilder {
  private readonly buffer: ArrayBuffer;
  private readonly view: DataView;

  public constructor(size: number = PlayStationRamSize) {
    this.buffer = new ArrayBuffer(size);
    this.view = new DataView(this.buffer);
  }

  private ensureWritable(
    address: number,
    length: number,
    value: number,
    min: number,
    max: number,
  ): void {
    if (!Number.isInteger(address) || address < 0 || address + length > this.buffer.byteLength) {
      throw new RangeError(
        `Address 0x${address.toString(16).toUpperCase()} (+${length} bytes) is outside the snapshot (${this.buffer.byteLength} bytes).`,
      );
    }

    if (!Number.isInteger(value) || value < min || value > max) {
      throw new RangeError(`Value ${value} does not fit in ${length} byte(s) (${min}..${max}).`);
    }
  }

  public writeByte(address: number, value: number): this {
    this.ensureWritable(address, 1, value, -0x80, 0xff);
    if (value < 0) {
      this.view.setInt8(address, value);
    } else {
      this.view.setUint8(address, value);
    }
    return this;
  }

  public writeInt16(address: number, value: number): this {
    this.ensureWritable(address, 2, value, -0x8000, 0xffff);
    if (value < 0) {
      this.view.setInt16(address, value, true);
    } else {
      this.view.setUint16(address, value, true);
    }
    return this;
  }

  public writeInt32(address: number, value: number): this {
    this.ensureWritable(address, 4, value, -0x80000000, 0xffffffff);
    if (value < 0) {
      this.view.setInt32(address, value, true);
    } else {
      this.view.setUint32(address, value, true);
    }
    return this;
  }

  public build(): ArrayBuffer {
    return this.buffer.slice(0);
  }
}
