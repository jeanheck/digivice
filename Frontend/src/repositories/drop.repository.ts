import DropJson from "@/database/drop/drop.json";
import type { DropTable } from "@/repositories/tables/drop/drop.table";
import type { DropRaw } from "@/repositories/tables/raws/drop/drop.raw";

export class DropRepository {
  private static readonly dropTable = DropJson as DropTable;

  public static getDropByKey(dropKey: string): DropRaw | undefined {
    return this.dropTable[dropKey];
  }

  public static getDropTable(): DropTable {
    return this.dropTable;
  }

  public static getDropKeys(): string[] {
    return Object.keys(this.dropTable);
  }
}
