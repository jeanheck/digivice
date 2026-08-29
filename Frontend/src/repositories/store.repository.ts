import StoreJson from "@/database/tcg/store.json";
import type { StoreTable } from "@/repositories/tables/tcg/store.table";
import type { StoreRaw } from "@/repositories/tables/raws/tcg/store.raw";

export class StoreRepository {
  private static readonly storeTable = StoreJson as StoreTable;

  public static getStoreById(storeId: string): StoreRaw | undefined {
    return this.storeTable[storeId];
  }

  public static getStoreTable(): StoreTable {
    return this.storeTable;
  }

  public static getStoreIds(): string[] {
    return Object.keys(this.storeTable);
  }
}
