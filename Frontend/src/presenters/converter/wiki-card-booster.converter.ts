import type { CardBoosterSourceViewModel } from "@/viewmodels/card/card-booster-source.viewmodel";
import type { BoosterViewModel } from "@/viewmodels/card/booster.viewmodel";

export class WikiCardBoosterConverter {
  public static convert(source: CardBoosterSourceViewModel): BoosterViewModel {
    return {
      dropKey: source.dropKey,
      labelKey: `boosters.${source.boosterId}.name`,
    };
  }
}
