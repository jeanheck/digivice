import type { NpcCharismaRequiredRaw } from "@/repositories/tables/raws/npc/npc-charisma-required.raw";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";

export class NpcService {
  public static isCharismaInRange(
    partyCharisma: number,
    charismaRequired: NpcCharismaRequiredRaw,
  ): boolean {
    if (partyCharisma < charismaRequired.min) {
      return false;
    }

    if (charismaRequired.max !== undefined && partyCharisma > charismaRequired.max) {
      return false;
    }

    return true;
  }

  public static hasAvailableBattle(npc: NpcRaw, partyCharisma: number): boolean {
    const cardBattles = npc.cardBattles ?? [];
    const digimonBattles = npc.digimonBattles ?? [];

    const hasAvailableCardBattle = cardBattles.some((cardBattle) => {
      return this.isCharismaInRange(partyCharisma, cardBattle.charismaRequired);
    });
    if (hasAvailableCardBattle) {
      return true;
    }

    return digimonBattles.some((digimonBattle) => {
      return this.isCharismaInRange(partyCharisma, digimonBattle.charismaRequired);
    });
  }
}
