import type { ImportantItems } from "@/models";
import type { ImportantItemsDTO } from "@/events/dto/important-items.dto";

export class ImportantItemsSyncer {
  public static sync(previousImportantItems: ImportantItems, newImportantItemsDto: ImportantItemsDTO): void {
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
