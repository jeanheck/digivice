import { WikiNpcFolderCardConverter } from "@/presenters/converter/wiki-npc-folder-card.converter";
import { CardRepository } from "@/repositories/card.repository";
import { FolderRepository } from "@/repositories/folder.repository";
import { NpcBattleOpponentHelper } from "@/presenters/helper/npc-battle-opponent.helper";
import type { WikiNpcCardBattleViewModel } from "@/viewmodels/wiki-modal/wiki-npc-card-battle.viewmodel";
import type { WikiNpcFolderCardViewModel } from "@/viewmodels/wiki-modal/wiki-npc-folder-card.viewmodel";

export class WikiNpcCardBattlePresenter {
  public static getBattleViewModel(
    npcId: string,
    battleId: string,
  ): WikiNpcCardBattleViewModel | null {
    const opponent = NpcBattleOpponentHelper.resolveById(npcId);
    if (opponent === undefined || opponent.source === "npc") {
      return null;
    }

    const cardBattle = opponent.raw.cardBattles?.[battleId];
    if (cardBattle === undefined) {
      return null;
    }

    const folderRaw = FolderRepository.getFolderById(cardBattle.folderId);
    if (folderRaw === undefined) {
      return null;
    }

    const cards: WikiNpcFolderCardViewModel[] = [];

    for (const folderCardRaw of folderRaw.cards) {
      if (CardRepository.getCardById(folderCardRaw.id) === undefined) {
        continue;
      }

      cards.push(WikiNpcFolderCardConverter.convert(folderCardRaw));
    }

    return {
      nameKey: `folder.${cardBattle.folderId}`,
      level: folderRaw.level,
      cards,
      drops: [
        {
          dropId: cardBattle.boosterId,
          type: "booster",
        },
      ],
    };
  }
}
