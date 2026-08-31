import { ImageCatalog } from "@/catalogs/image.catalog";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import { EnemyRepository } from "@/repositories/enemy.repository";
import type { NpcPartyMemberRaw } from "@/repositories/tables/raws/npc/npc-party-member.raw";
import type { WikiNpcDigimonBattleViewModel } from "@/viewmodels/wiki-modal/wiki-npc-digimon-battle.viewmodel";

export class WikiNpcDigimonBattlePresenter {
  private static buildMembers(party: NpcPartyMemberRaw[]) {
    return party.flatMap((partyMember) => {
      const enemyRaw = EnemyRepository.getEnemyByMemoryIdAndGroupId(
        partyMember.enemyId,
        partyMember.groupId,
      );
      if (enemyRaw === null) {
        return [];
      }

      const enemy = EnemyConverter.convert(enemyRaw);

      return [
        {
          id: String(partyMember.enemyId),
          enemy,
          imageUrl: ImageCatalog.getEnemyIconUrl(enemy.name),
        },
      ];
    });
  }

  public static getBattleViewModel(
    npcId: string,
    battleId: string,
  ): WikiNpcDigimonBattleViewModel | null {
    const opponent = NpcBattleOpponentHelper.resolveById(npcId);
    if (opponent === undefined) {
      return null;
    }

    if (opponent.source === "npc") {
      const members = this.buildMembers(opponent.raw.party);

      return {
        exp: opponent.raw.exp,
        dvexp: opponent.raw.dvexp,
        bits: opponent.raw.bit,
        partyMemberCount: members.length,
        members,
      };
    }

    const digimonBattle = opponent.raw.digimonBattles?.[battleId];
    if (digimonBattle === undefined) {
      return null;
    }

    const members = this.buildMembers(digimonBattle.party);

    return {
      exp: digimonBattle.exp,
      dvexp: digimonBattle.dvexp,
      bits: digimonBattle.bit,
      partyMemberCount: members.length,
      members,
    };
  }
}
