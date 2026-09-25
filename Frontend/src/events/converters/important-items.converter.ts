import type { ImportantItemsDTO } from "../dto/important-items.dto";
import type { ImportantItems } from "../../models";

export class ImportantItemsConverter {
  public static convert(importantItemsDto: Required<ImportantItemsDTO>): ImportantItems {
    return {
      treeBoots: importantItemsDto.treeBoots,
      fishingPole: importantItemsDto.fishingPole,
      asukaTrophy: importantItemsDto.asukaTrophy,
      sunTrophy: importantItemsDto.sunTrophy,
    };
  }
}
