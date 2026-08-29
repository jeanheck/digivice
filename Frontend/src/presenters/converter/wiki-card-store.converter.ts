import { StoreRepository } from "@/repositories/store.repository";
import type { WikiCardStoreViewModel } from "@/viewmodels/wiki-modal/wiki-card-store.viewmodel";

export class WikiCardStoreConverter {
  public static convert(storeId: string): WikiCardStoreViewModel | null {
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
