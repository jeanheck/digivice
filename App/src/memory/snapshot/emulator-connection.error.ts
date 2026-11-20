export class EmulatorConnectionError extends Error {
  public readonly code: string;
  public readonly detail: string | null;

  public constructor(code: string, detail: string | null = null) {
    super(detail === null ? code : `${code}: ${detail}`);
    this.name = "EmulatorConnectionError";
    this.code = code;
    this.detail = detail;
  }
}
