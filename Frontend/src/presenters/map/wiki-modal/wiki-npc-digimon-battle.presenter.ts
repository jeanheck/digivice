import { ImageCatalog } from "@/catalogs/image.catalog";
import { EnemyConverter } from "@/presenters/converter/enemy.converter";
import { EnemyRepository } from "@/repositories/enemy.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import type { WikiNpcDigimonBattleViewModel } from "@/viewmodels/wiki-modal/wiki-npc-digimon-battle.viewmodel";

export class WikiNpcDigimonBattlePresenter {
  public static getBattleViewModel(
    npcId: string,
    battleId: string,
  ): WikiNpcDigimonBattleViewModel | null {
    const npcRaw = NpcRepository.getNpcById(npcId);
    const digimonBattle = npcRaw?.digimonBattles?.[battleId];
    if (digimonBattle === undefined) {
      return null;
    }

    const members = digimonBattle.party.flatMap((partyMember) => {
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

    return {
      exp: digimonBattle.exp,
      dvexp: digimonBattle.dvexp,
      bits: digimonBattle.bit,
      partyMemberCount: members.length,
      members,
    };
  }
}
