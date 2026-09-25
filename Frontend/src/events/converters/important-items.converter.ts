import type { DeepRequired } from "@/events/dto/deep-required";
import type { ImportantItemsDTO } from "@/events/dto/important-items.dto";
import type { ImportantItems } from "@/models";

export class ImportantItemsConverter {
  public static convert(importantItemsDto: DeepRequired<ImportantItemsDTO>): ImportantItems {
    return {
      treeBoots: importantItemsDto.treeBoots,
      fishingPole: importantItemsDto.fishingPole,
      asukaTrophy: importantItemsDto.asukaTrophy,
      sunTrophy: importantItemsDto.sunTrophy,
    };
  }
}
