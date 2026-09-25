import {
  NpcBattleKindConstant,
  STORY_NPC_DIGIMON_BATTLE_ID,
} from "@/constants/npc-battle-kind.constant";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { EnemyRepository } from "@/repositories/enemy.repository";
import type { NpcPartyMemberRaw } from "@/repositories/tables/raws/npc/npc-party-member.raw";
import type { TamerDigimonBattlePartyMemberRaw } from "@/repositories/tables/raws/tamer/tamer-digimon-battle-party-member.raw";
import type { TamerDigimonBattleRaw } from "@/repositories/tables/raws/tamer/tamer-digimon-battle.raw";

export interface NpcBattleFromEnemyContext {
  npcId: string;
  battleOptionId: string | null;
}

export class NpcBattleFromEnemyHelper {
  private static isPartyMemberMatch(
    partyMember: NpcPartyMemberRaw | TamerDigimonBattlePartyMemberRaw,
    memoryId: number,
    groupId: number,
  ): boolean {
    return partyMember.enemyId === memoryId && partyMember.groupId === groupId;
  }

  private static findDigimonBattleId(
    digimonBattles: Record<string, TamerDigimonBattleRaw> | undefined,
    memoryId: number,
    groupId: number,
  ): string | null {
    if (digimonBattles === undefined) {
      return null;
    }

    for (const [battleId, digimonBattle] of Object.entries(digimonBattles)) {
      const hasMatch = digimonBattle.party.some((partyMember) => {
        return this.isPartyMemberMatch(partyMember, memoryId, groupId);
      });
      if (hasMatch) {
        return battleId;
      }
    }

    return null;
  }

  public static resolve(enemyId: string): NpcBattleFromEnemyContext | null {
    const enemyRaw = EnemyRepository.getEnemyById(enemyId);
    const opponentId = enemyRaw.tamerId ?? enemyRaw.npcId;
    if (opponentId === undefined) {
      return null;
    }

    const opponent = NpcBattleOpponentHelper.resolveById(opponentId);
    if (opponent === undefined) {
      return null;
    }

    if (opponent.source === "npc") {
      return {
        npcId: opponentId,
        battleOptionId: `${NpcBattleKindConstant.digimon}-${STORY_NPC_DIGIMON_BATTLE_ID}`,
      };
    }

    if (enemyRaw.groupId === null) {
      return {
        npcId: opponentId,
        battleOptionId: null,
      };
    }

    const battleId = this.findDigimonBattleId(
      opponent.raw.digimonBattles,
      enemyRaw.memoryId,
      enemyRaw.groupId,
    );

    return {
      npcId: opponentId,
      battleOptionId:
        battleId !== null ? `${NpcBattleKindConstant.digimon}-${battleId}` : null,
    };
  }
}
