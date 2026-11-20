import { defineStore } from "pinia";
import { ref } from "vue";
import type { HealthStatus, State } from "@/state/models";
import { StatePatcher } from "@/memory/patch/state.patcher";

export const useGameStore = defineStore("game", () => {
  const healthStatus = ref<HealthStatus>("Loading");
  const lastErrorCode = ref<string | null>(null);
  const lastErrorDetail = ref<string | null>(null);
  const currentState = ref<State | null>(null);
  let statePatcher = new StatePatcher();

  function applyState(state: State): void {
    if (currentState.value === null) {
      statePatcher = new StatePatcher();
      currentState.value = state;
    } else {
      statePatcher.patch(currentState.value, state);
    }

    healthStatus.value = "Healthy";
    lastErrorCode.value = null;
    lastErrorDetail.value = null;
  }

  function applyError(code: string, detail: string | null): void {
    currentState.value = null;
    healthStatus.value = "Error";
    lastErrorCode.value = code;
    lastErrorDetail.value = detail;
  }

  return {
    healthStatus,
    lastErrorCode,
    lastErrorDetail,
    currentState,
    applyState,
    applyError,
  };
});
