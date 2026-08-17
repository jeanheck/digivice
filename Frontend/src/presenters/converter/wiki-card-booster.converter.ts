import type { CardBoosterSourceViewModel } from "@/viewmodels/card/card-booster-source.viewmodel";
import type { WikiCardBoosterViewModel } from "@/viewmodels/wiki-modal/wiki-card-booster.viewmodel";

export class WikiCardBoosterConverter {
  public static convert(source: CardBoosterSourceViewModel): WikiCardBoosterViewModel {
    return {
      dropKey: source.dropKey,
      labelKey: `boosters.${source.boosterId}.name`,
    };
  }
}
