export class MemoryReadError extends Error {
  public readonly address: number;

  public constructor(address: number, message: string) {
    super(message);
    this.name = "MemoryReadError";
    this.address = address;
  }
}
