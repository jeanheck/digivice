import type { DigimonBattle, Enemy } from "@/state/models";
import type { EnemyResource } from "@/memory/resources/battles/enemy.resource";
import type { DigimonBattleResource } from "@/memory/resources/digimon-battle.resource";

export class DigimonBattleAssembler {
  private static assembleEnemy(resource: EnemyResource): Enemy | null {
    if (resource.id <= 0) {
      return null;
    }

    return {
      id: resource.id,
      groupId: resource.groupId,
      condition: resource.condition,
      strength: resource.strength,
      defense: resource.defense,
      speed: resource.speed,
      hp: {
        current: resource.hp.current,
        max: resource.hp.max,
      },
    };
  }

  public static assemble(resource: DigimonBattleResource): DigimonBattle {
    return {
      field: resource.field,
      enemy: DigimonBattleAssembler.assembleEnemy(resource.enemy),
    };
  }
}
