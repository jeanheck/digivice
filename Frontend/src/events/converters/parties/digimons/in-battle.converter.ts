import type { InBattleDTO } from "@/events/dto/parties/digimons/in-battle.dto";
import type { InBattle } from "@/models/party/digimon/in-battle";
import { VitalConverter } from "./vital.converter";

export class InBattleConverter {
  public static convert(newInBattleDto: InBattleDTO | null): InBattle {
    return {
      condition: newInBattleDto?.condition ?? 0,
      strength: newInBattleDto?.strength ?? 0,
      defense: newInBattleDto?.defense ?? 0,
      speed: newInBattleDto?.speed ?? 0,
      hp: VitalConverter.convert(newInBattleDto?.hp ?? null),
      mp: VitalConverter.convert(newInBattleDto?.mp ?? null),
    };
  }
}
