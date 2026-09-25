import type { InBattleDTO } from "@/events/dto/parties/digimons/in-battle.dto";
import type { InBattle } from "@/models";
import { VitalSyncer } from "./vital.syncer";

export class InBattleSyncer {
  public static sync(previousInBattle: InBattle, newInBattleDto: InBattleDTO): void {
    if (newInBattleDto.condition !== undefined) {
      previousInBattle.condition = newInBattleDto.condition;
    }
    if (newInBattleDto.strength !== undefined) {
      previousInBattle.strength = newInBattleDto.strength;
    }
    if (newInBattleDto.defense !== undefined) {
      previousInBattle.defense = newInBattleDto.defense;
    }
    if (newInBattleDto.speed !== undefined) {
      previousInBattle.speed = newInBattleDto.speed;
    }
    if (newInBattleDto.hp) {
      VitalSyncer.sync(previousInBattle.hp, newInBattleDto.hp);
    }
    if (newInBattleDto.mp) {
      VitalSyncer.sync(previousInBattle.mp, newInBattleDto.mp);
    }
  }
}
