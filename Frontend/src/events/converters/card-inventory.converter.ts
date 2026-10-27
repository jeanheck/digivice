import type { DeepRequired } from "@/events/dto/deep-required";
import type { CardInventoryDTO } from "@/events/dto/card-inventory.dto";
import type { CardInventory } from "@/models";

export class CardInventoryConverter {
  public static convert(cardInventoryDto: DeepRequired<CardInventoryDTO>): CardInventory {
    return {
      cards: { ...cardInventoryDto.cards },
    };
  }
}
