import { CardShopRepository } from "@/repositories/card-shop.repository";
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";

export class WikiCardShopConverter {
  public static convert(cardShopId: string): CardShopViewModel | null {
    const cardShopRaw = CardShopRepository.getById(cardShopId);
    if (cardShopRaw === undefined) {
      return null;
    }

    return {
      id: cardShopId,
      labelKey: `cardShops.${cardShopId}.name`,
    };
  }
}
