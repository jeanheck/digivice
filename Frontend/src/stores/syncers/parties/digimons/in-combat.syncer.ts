import type { InCombatDTO } from "@/events/dto/parties/digimons/in-combat.dto";
import type { InCombat } from "@/models";
import { VitalSyncer } from "./vital.syncer";

export class InCombatSyncer {
  public static sync(previousInCombat: InCombat, newInCombatDto: InCombatDTO): void {
    if (newInCombatDto.condition !== undefined) {
      previousInCombat.condition = newInCombatDto.condition;
    }
    if (newInCombatDto.hp) {
      VitalSyncer.sync(previousInCombat.hp, newInCombatDto.hp);
    }
    if (newInCombatDto.mp) {
      VitalSyncer.sync(previousInCombat.mp, newInCombatDto.mp);
    }
  }
}
