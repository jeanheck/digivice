import type { EnemyDTO } from "@/events/dto/battles/enemy.dto";
import type { Enemy } from "@/models/battle/enemy";
import { VitalSyncer } from "../parties/digimons/vital.syncer";

export class EnemySyncer {
  public static sync(previousEnemy: Enemy, newEnemyDto: EnemyDTO): void {
    if (newEnemyDto.id !== undefined) {
      previousEnemy.id = newEnemyDto.id;
    }
    if (newEnemyDto.condition !== undefined) {
      previousEnemy.condition = newEnemyDto.condition;
    }
    if (newEnemyDto.strength !== undefined) {
      previousEnemy.strength = newEnemyDto.strength;
    }
    if (newEnemyDto.defense !== undefined) {
      previousEnemy.defense = newEnemyDto.defense;
    }
    if (newEnemyDto.speed !== undefined) {
      previousEnemy.speed = newEnemyDto.speed;
    }
    if (newEnemyDto.hp) {
      VitalSyncer.sync(previousEnemy.hp, newEnemyDto.hp);
    }
  }
}
