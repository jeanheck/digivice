import type { InCombatDTO } from "@/events/dto/parties/digimons/in-combat.dto";
import type { InCombat } from "@/models/party/digimon/in-combat";
import { VitalConverter } from "./vital.converter";

export class InCombatConverter {
  public static convert(newInCombatDto: InCombatDTO | null): InCombat {
    return {
      condition: newInCombatDto?.condition ?? 0,
      hp: VitalConverter.convert(newInCombatDto?.hp ?? null),
      mp: VitalConverter.convert(newInCombatDto?.mp ?? null),
    };
  }
}
