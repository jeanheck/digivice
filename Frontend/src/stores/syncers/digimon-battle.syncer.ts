import type { DigimonBattle } from "@/models";
import type { DigimonBattleDTO } from "@/events/dto/digimon-battle.dto";
import type { EnemyDTO } from "@/events/dto/battles/enemy.dto";
import type { DeepRequired } from "@/events/dto/deep-required";
import { EnemyConverter } from "@/events/converters/battles/enemy.converter";
import { EnemySyncer } from "./battles/enemy.syncer";

export class DigimonBattleSyncer {
  public static sync(previousDigimonBattle: DigimonBattle, newDigimonBattleDto: DigimonBattleDTO): void {
    if (newDigimonBattleDto.field !== undefined) {
      previousDigimonBattle.field = newDigimonBattleDto.field;
    }
    if (newDigimonBattleDto.enemy === null) {
      previousDigimonBattle.enemy = null;
    } else if (newDigimonBattleDto.enemy !== undefined) {
      if (previousDigimonBattle.enemy === null) {
        // Backend sends the full enemy when it appears.
        previousDigimonBattle.enemy = EnemyConverter.convert(
          newDigimonBattleDto.enemy as DeepRequired<EnemyDTO>,
        );
      } else {
        EnemySyncer.sync(previousDigimonBattle.enemy, newDigimonBattleDto.enemy);
      }
    }
  }
}
