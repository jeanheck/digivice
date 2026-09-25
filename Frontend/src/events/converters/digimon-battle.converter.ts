import type { DeepRequired } from "@/events/dto/deep-required";
import type { DigimonBattleDTO } from "@/events/dto/digimon-battle.dto";
import type { DigimonBattle } from "@/models/digimon-battle";
import { EnemyConverter } from "./battles/enemy.converter";

export class DigimonBattleConverter {
  public static convert(digimonBattleDto: DeepRequired<DigimonBattleDTO>): DigimonBattle {
    return {
      field: digimonBattleDto.field,
      enemy: digimonBattleDto.enemy ? EnemyConverter.convert(digimonBattleDto.enemy) : null,
    };
  }
}
