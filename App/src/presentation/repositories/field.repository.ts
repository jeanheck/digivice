import FieldJson from "@/data/game/field.json";
import type { FieldTable } from "@/presentation/repositories/tables/battle/field.table";
import type { FieldRaw } from "@/presentation/repositories/tables/raws/battle/field.raw";

export class FieldRepository {
  private static readonly fieldTable = FieldJson as FieldTable;

  public static getFieldById(fieldId: number): FieldRaw | null {
    return this.fieldTable[fieldId] ?? null;
  }
}
