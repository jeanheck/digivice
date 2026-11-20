import { computed, watch } from "vue";
import { useGameStore } from "@/state/stores/use-game-store";

export function useBootLoader() {
  const store = useGameStore();
  const isLoading = computed(() => store.healthStatus === "Loading");

  watch(
    isLoading,
    (loading) => {
      if (!loading) {
        document.getElementById("boot-loader")?.remove();
      }
    },
    { immediate: true },
  );

  return isLoading;
}
