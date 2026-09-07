import { StoreRepository } from "@/repositories/store.repository";
import type { CardShopViewModel } from "@/viewmodels/card/card-shop.viewmodel";

export class WikiCardStoreConverter {
  public static convert(storeId: string): CardShopViewModel | null {
    const storeRaw = StoreRepository.getStoreById(storeId);
    if (storeRaw === undefined) {
      return null;
    }

    return {
      storeId,
      labelKey: `stores.${storeId}.name`,
    };
  }
}
