export class SaveNotLoadedError extends Error {
  public constructor(message: string) {
    super(message);
    this.name = "SaveNotLoadedError";
  }
}
