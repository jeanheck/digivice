import type { ImportantItems } from "../../models";
import type * as Events from "../../events/events.map";

export class ImportantItemsSyncer {
  public static sync(previousImportantItems: ImportantItems, newImportantItemsDto: Events.ImportantItemsDTO): void {
    if (newImportantItemsDto.treeBoots !== undefined) {
      previousImportantItems.treeBoots = newImportantItemsDto.treeBoots;
    }
    if (newImportantItemsDto.fishingPole !== undefined) {
      previousImportantItems.fishingPole = newImportantItemsDto.fishingPole;
    }
    if (newImportantItemsDto.asukaTrophy !== undefined) {
      previousImportantItems.asukaTrophy = newImportantItemsDto.asukaTrophy;
    }
    if (newImportantItemsDto.sunTrophy !== undefined) {
      previousImportantItems.sunTrophy = newImportantItemsDto.sunTrophy;
    }
  }
}
