import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";

// Only valid inside the main tree, which App.vue renders exclusively while currentState exists.
export function useGameState() {
  const store = useGameStore();

  return computed(() => {
    const state = store.currentState;
    if (!state) {
      throw new Error("Game state accessed before InitialState.");
    }

    return state;
  });
}
