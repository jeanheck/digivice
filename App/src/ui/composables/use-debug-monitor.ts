import { computed, onMounted, onUnmounted, ref, watch } from "vue";
import { useGameStore } from "@/state/stores/use-game-store";

const OpenStorageKey = "debug-monitor-open";
const ToggleKey = "F2";

export function useDebugMonitor() {
  const store = useGameStore();
  const isOpen = ref(localStorage.getItem(OpenStorageKey) === "true");
  const lastChangedAt = ref<Date | null>(null);

  const healthStatus = computed(() => store.healthStatus);
  const lastErrorCode = computed(() => store.lastErrorCode);
  const lastErrorDetail = computed(() => store.lastErrorDetail);
  const currentState = computed(() => store.currentState);
  function toggle(): void {
    isOpen.value = !isOpen.value;
    localStorage.setItem(OpenStorageKey, String(isOpen.value));
  }

  function onKeyDown(event: KeyboardEvent): void {
    if (event.key !== ToggleKey) {
      return;
    }

    event.preventDefault();
    toggle();
  }

  watch(
    () => store.currentState,
    () => {
      lastChangedAt.value = new Date();
    },
    { deep: true },
  );

  onMounted(() => window.addEventListener("keydown", onKeyDown));
  onUnmounted(() => window.removeEventListener("keydown", onKeyDown));

  return {
    isOpen,
    toggle,
    healthStatus,
    lastErrorCode,
    lastErrorDetail,
    currentState,
    lastChangedAt,
  };
}
