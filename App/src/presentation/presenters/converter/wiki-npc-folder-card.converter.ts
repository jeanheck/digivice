import { CardRepository } from "@/presentation/repositories/card.repository";
import type { FolderCardRaw } from "@/presentation/repositories/tables/raws/tcg/folder.raw";
import type { WikiNpcFolderCardViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-npc-folder-card.viewmodel";

export class WikiNpcFolderCardConverter {
  public static convert(folderCardRaw: FolderCardRaw): WikiNpcFolderCardViewModel {
    const cardRaw = CardRepository.getCardById(folderCardRaw.id);

    return {
      cardId: folderCardRaw.id,
      imageName: cardRaw?.imageName ?? "",
      nameKey: `card.${folderCardRaw.id}.name`,
      quantity: folderCardRaw.quantity,
    };
  }
}
