import type { CardInventory } from "@/models";
import type { CardInventoryDTO } from "@/events/dto/card-inventory.dto";

export class CardInventorySyncer {
  public static sync(previousCardInventory: CardInventory, newCardInventoryDto: CardInventoryDTO): void {
    if (newCardInventoryDto.cards !== undefined) {
      previousCardInventory.cards = { ...newCardInventoryDto.cards };
    }
  }
}
