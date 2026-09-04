import type { DigimonBattle } from "@/models/digimon-battle";
import type * as Events from "@/events/events.map";
import { EnemySyncer } from "./battles/enemy.syncer";

export class DigimonBattleSyncer {
  public static sync(previousDigimonBattle: DigimonBattle, newDigimonBattleDto: Events.DigimonBattleDTO): void {
    if (newDigimonBattleDto.field !== undefined) {
      previousDigimonBattle.field = newDigimonBattleDto.field;
    }
    if (newDigimonBattleDto.enemy) {
      EnemySyncer.sync(previousDigimonBattle.enemy, newDigimonBattleDto.enemy);
    }
  }
}
