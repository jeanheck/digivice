import { defineStore } from "pinia";
import { ref } from "vue";

const PinnedFamiliesStorageKey = "digievolution-pinned-families";

function loadPinnedFamilies(): Record<string, string> {
  const storedValue = localStorage.getItem(PinnedFamiliesStorageKey);
  if (storedValue === null) {
    return {};
  }

  try {
    const parsedValue: unknown = JSON.parse(storedValue);
    if (typeof parsedValue !== "object" || parsedValue === null || Array.isArray(parsedValue)) {
      return {};
    }

    return parsedValue as Record<string, string>;
  } catch {
    return {};
  }
}

export const usePreferencesStore = defineStore("preferences", () => {
  const pinnedFamilyKeyByDigimonId = ref<Record<string, string>>(loadPinnedFamilies());

  function getPinnedFamilyKey(digimonId: number): string | null {
    return pinnedFamilyKeyByDigimonId.value[String(digimonId)] ?? null;
  }

  function togglePinnedFamily(digimonId: number, familyKey: string): void {
    const digimonKey = String(digimonId);

    if (pinnedFamilyKeyByDigimonId.value[digimonKey] === familyKey) {
      delete pinnedFamilyKeyByDigimonId.value[digimonKey];
    } else {
      pinnedFamilyKeyByDigimonId.value[digimonKey] = familyKey;
    }

    localStorage.setItem(PinnedFamiliesStorageKey, JSON.stringify(pinnedFamilyKeyByDigimonId.value));
  }

  return {
    pinnedFamilyKeyByDigimonId,
    getPinnedFamilyKey,
    togglePinnedFamily,
  };
});
