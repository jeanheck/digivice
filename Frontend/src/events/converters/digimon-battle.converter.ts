import type { DigimonBattleDTO } from "@/events/dto/digimon-battle.dto";
import type { DigimonBattle } from "@/models/digimon-battle";
import { EnemyConverter } from "./battles/enemy.converter";

export class DigimonBattleConverter {
  public static convert(digimonBattleDto: Required<DigimonBattleDTO>): DigimonBattle {
    return {
      field: digimonBattleDto.field ?? 0,
      enemy: EnemyConverter.convert(digimonBattleDto.enemy ?? null),
    };
  }
}
