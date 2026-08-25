import { ImageCatalog } from "@/catalogs/image.catalog";
import { NpcPartyDigimonConverter } from "@/presenters/converter/npc-party-digimon.converter";
import { NpcPartyRepository } from "@/repositories/npc-party.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import type { WikiNpcDigimonBattleViewModel } from "@/viewmodels/wiki-modal/wiki-npc-digimon-battle.viewmodel";

export class WikiNpcDigimonBattlePresenter {
  public static getBattleViewModel(
    npcId: string,
    battleIndex: number,
  ): WikiNpcDigimonBattleViewModel | null {
    const npcRaw = NpcRepository.getNpcById(npcId);
    const digimonBattle = npcRaw?.digimonBattles?.[battleIndex];
    if (digimonBattle === undefined) {
      return null;
    }

    const partyRaw = NpcPartyRepository.getPartyById(digimonBattle.npcPartyId);
    const members =
      partyRaw === undefined
        ? []
        : Object.entries(partyRaw).map(([memberId, partyDigimonRaw]) => {
            const enemy = NpcPartyDigimonConverter.convert(partyDigimonRaw);

            return {
              id: memberId,
              enemy,
              imageUrl: ImageCatalog.getEnemyIconUrl(enemy.name),
            };
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
