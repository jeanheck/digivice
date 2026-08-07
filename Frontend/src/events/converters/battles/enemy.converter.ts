import type { EnemyDTO } from "@/events/dto/battles/enemy.dto";
import type { Enemy } from "@/models/battle/enemy";
import { VitalConverter } from "../parties/digimons/vital.converter";

export class EnemyConverter {
  public static convert(newEnemyDto: EnemyDTO | null): Enemy {
    return {
      id: newEnemyDto?.id ?? 0,
      condition: newEnemyDto?.condition ?? 0,
      strength: newEnemyDto?.strength ?? 0,
      defense: newEnemyDto?.defense ?? 0,
      speed: newEnemyDto?.speed ?? 0,
      hp: VitalConverter.convert(newEnemyDto?.hp ?? null),
    };
  }
}
