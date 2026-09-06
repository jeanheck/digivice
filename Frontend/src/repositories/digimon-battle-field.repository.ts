import DigimonBattleFieldJson from "@/database/digimon-battle-field/digimon-battle-field.json";
import type { DigimonBattleFieldTable } from "@/repositories/tables/digimon-battle-field/digimon-battle-field.table";
import type { DigimonBattleFieldRaw } from "@/repositories/tables/raws/digimon-battle-field/digimon-battle-field.raw";

export class DigimonBattleFieldRepository {
  private static readonly digimonBattleFieldTable = DigimonBattleFieldJson as DigimonBattleFieldTable;

  public static getByFieldId(fieldId: number): DigimonBattleFieldRaw | null {
    return this.digimonBattleFieldTable[fieldId] ?? null;
  }
}
