import type { BattleDTO } from "@/events/dto/battle.dto";
import type { Battle } from "@/models/battle";
import { EnemyConverter } from "./battles/enemy.converter";

export class BattleConverter {
  public static convert(battleDto: Required<BattleDTO>): Battle {
    return {
      field: battleDto.field ?? 0,
      groupId: battleDto.groupId ?? 0,
      enemy: EnemyConverter.convert(battleDto.enemy ?? null),
    };
  }
}
