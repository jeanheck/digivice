import FieldJson from "@/database/field.json";
import type { FieldTable } from "@/repositories/tables/battle/field.table";
import type { FieldRaw } from "@/repositories/tables/raws/battle/field.raw";

export class FieldRepository {
  private static readonly fieldTable = FieldJson as FieldTable;

  public static getFieldById(fieldId: number): FieldRaw | null {
    return this.fieldTable[fieldId] ?? null;
  }
}
