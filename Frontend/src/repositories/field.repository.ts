import FieldJson from "@/database/fields/field.json";
import { resolveBattleFieldElement } from "@/constants/battle-field.constant";
import type { FieldTable } from "@/repositories/tables/field/field.table";
import type { FieldRaw } from "@/repositories/tables/raws/field/field.raw";

export class FieldRepository {
  private static readonly fieldTable = FieldJson as FieldTable;

  public static getFieldTable(): FieldTable {
    return this.fieldTable;
  }

  public static getByElement(element: string): FieldRaw | undefined {
    return this.fieldTable[element];
  }

  public static getByFieldId(fieldId: number): FieldRaw | null {
    const element = resolveBattleFieldElement(fieldId);
    if (element === null) {
      return null;
    }

    return this.fieldTable[element] ?? null;
  }
}
