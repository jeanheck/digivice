import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import type { NpcPartyMemberRaw } from "@/repositories/tables/raws/npc/npc-party-member.raw";
import type { TamerDigimonBattleRaw } from "@/repositories/tables/raws/tamer/tamer-digimon-battle.raw";

export class EnemyOwnerRepository {
  private static readonly ownerIdsByEnemyKey = this.buildOwnerIdsByEnemyKey();

  private static buildOwnerIdsByEnemyKey(): Map<string, string[]> {
    const ownerIdsByEnemyKey = new Map<string, string[]>();

    const addParty = (ownerId: string, party: NpcPartyMemberRaw[]) => {
      for (const partyMember of party) {
        const enemyKey = this.toEnemyKey(partyMember.enemyId, partyMember.groupId);
        const ownerIds = ownerIdsByEnemyKey.get(enemyKey) ?? [];
        if (!ownerIds.includes(ownerId)) {
          ownerIds.push(ownerId);
        }
        ownerIdsByEnemyKey.set(enemyKey, ownerIds);
      }
    };

    const addDigimonBattles = (
      ownerId: string,
      digimonBattles: Record<string, TamerDigimonBattleRaw> | undefined,
    ) => {
      for (const digimonBattle of Object.values(digimonBattles ?? {})) {
        addParty(ownerId, digimonBattle.party);
      }
    };

    for (const [tamerId, tamerRaw] of Object.entries(TamerRepository.getTamerTable())) {
      addDigimonBattles(tamerId, tamerRaw.digimonBattles);
    }

    for (const [duelIslandId, duelIslandRaw] of Object.entries(
      DuelIslandRepository.getDuelIslandTable(),
    )) {
      addDigimonBattles(duelIslandId, duelIslandRaw.digimonBattles);
    }

    for (const [npcId, npcRaw] of Object.entries(NpcRepository.getNpcTable())) {
      addParty(npcId, npcRaw.party);
    }

    return ownerIdsByEnemyKey;
  }

  private static toEnemyKey(memoryId: number, groupId: number): string {
    return `${memoryId}:${groupId}`;
  }

  public static getOwnerId(memoryId: number | null, groupId: number | null): string | null {
    if (memoryId === null || groupId === null) {
      return null;
    }

    return this.ownerIdsByEnemyKey.get(this.toEnemyKey(memoryId, groupId))?.[0] ?? null;
  }
}
