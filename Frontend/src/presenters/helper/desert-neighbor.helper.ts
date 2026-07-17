import type { DesertNeighborNameViewModel } from "@/viewmodels/desert/desert-neighbor-name.viewmodel";

export class DesertNeighborHelper {
  private static readonly translationKeyByNeighbor: Record<string, string> = {
    noiseDesertS: "location.0257",
    mirageTower: "location.025A",
  };

  public static resolveNeighborName(neighbor: string): DesertNeighborNameViewModel {
    const translationKey = DesertNeighborHelper.translationKeyByNeighbor[neighbor];

    if (translationKey === undefined) {
      return { kind: "literal", value: neighbor };
    }

    return { kind: "i18n", key: translationKey };
  }
}
