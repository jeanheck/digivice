import type { DigimonBattle } from "@/models/digimon-battle";
import type * as Events from "@/events/events.map";
import { EnemyConverter } from "@/events/converters/battles/enemy.converter";
import { EnemySyncer } from "./battles/enemy.syncer";

export class DigimonBattleSyncer {
  public static sync(previousDigimonBattle: DigimonBattle, newDigimonBattleDto: Events.DigimonBattleDTO): void {
    if (newDigimonBattleDto.field !== undefined) {
      previousDigimonBattle.field = newDigimonBattleDto.field;
    }
    if (newDigimonBattleDto.enemy === null) {
      previousDigimonBattle.enemy = null;
    } else if (newDigimonBattleDto.enemy !== undefined) {
      if (previousDigimonBattle.enemy === null) {
        previousDigimonBattle.enemy = EnemyConverter.convert(newDigimonBattleDto.enemy);
      } else {
        EnemySyncer.sync(previousDigimonBattle.enemy, newDigimonBattleDto.enemy);
      }
    }
  }
}
