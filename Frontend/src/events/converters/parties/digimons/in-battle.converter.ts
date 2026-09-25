import type { DeepRequired } from "@/events/dto/deep-required";
import type { InBattleDTO } from "@/events/dto/parties/digimons/in-battle.dto";
import type { InBattle } from "@/models/party/digimon/in-battle";
import { VitalConverter } from "./vital.converter";

export class InBattleConverter {
  public static convert(inBattleDto: DeepRequired<InBattleDTO>): InBattle {
    return {
      condition: inBattleDto.condition,
      strength: inBattleDto.strength,
      defense: inBattleDto.defense,
      speed: inBattleDto.speed,
      hp: VitalConverter.convert(inBattleDto.hp),
      mp: VitalConverter.convert(inBattleDto.mp),
    };
  }
}
