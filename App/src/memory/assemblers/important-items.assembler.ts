import type { ImportantItems } from "@/state/models";
import type { ImportantItemsResource } from "@/memory/resources/important-items.resource";

export class ImportantItemsAssembler {
  public static assemble(resource: ImportantItemsResource): ImportantItems {
    return {
      treeBoots: resource.treeBoots !== 0,
      fishingPole: resource.fishingPole !== 0,
      asukaTrophy: resource.asukaTrophy !== 0,
      sunTrophy: resource.sunTrophy !== 0,
    };
  }
}
