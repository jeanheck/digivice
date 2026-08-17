import { IconConstant } from "@/constants/icon.constant";
import type { Constant } from "@/constants/constant";
import type { EnemyLocationViewModel } from "@/viewmodels/enemy/enemy-location.viewmodel";
import type { WikiLocationViewModel } from "@/viewmodels/wiki-modal/wiki-location.viewmodel";

export class WikiLocationConverter {
  public static convert(location: EnemyLocationViewModel): WikiLocationViewModel {
    return {
      id: location.id,
      labelKey: `location.${location.id}`,
      sources: location.sources.map((source) => {
        return {
          icon: IconConstant[source as Constant],
          ariaLabelKey: `enemy.locationSource.${source}`,
        };
      }),
    };
  }
}
