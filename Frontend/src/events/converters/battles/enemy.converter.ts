import type { DeepRequired } from "@/events/dto/deep-required";
import type { EnemyDTO } from "@/events/dto/battles/enemy.dto";
import type { Enemy } from "@/models";
import { VitalConverter } from "@/events/converters/parties/digimons/vital.converter";

export class EnemyConverter {
  public static convert(enemyDto: DeepRequired<EnemyDTO>): Enemy {
    return {
      id: enemyDto.id,
      groupId: enemyDto.groupId,
      condition: enemyDto.condition,
      strength: enemyDto.strength,
      defense: enemyDto.defense,
      speed: enemyDto.speed,
      hp: VitalConverter.convert(enemyDto.hp),
    };
  }
}
