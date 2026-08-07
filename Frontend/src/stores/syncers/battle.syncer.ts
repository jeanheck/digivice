import type { Battle } from "@/models/battle";
import type * as Events from "@/events/events.map";
import { EnemySyncer } from "./battles/enemy.syncer";

export class BattleSyncer {
  public static sync(previousBattle: Battle, newBattleDto: Events.BattleDTO): void {
    if (newBattleDto.enemy) {
      EnemySyncer.sync(previousBattle.enemy, newBattleDto.enemy);
    }
  }
}
