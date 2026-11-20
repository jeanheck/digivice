<script setup lang="ts">
import { useDebugMonitor } from "@/ui/composables/use-debug-monitor";
import DebugHeader from "@/ui/components/debug-monitor/DebugHeader.vue";
import DebugPlayer from "@/ui/components/debug-monitor/DebugPlayer.vue";
import DebugParty from "@/ui/components/debug-monitor/DebugParty.vue";
import DebugBattle from "@/ui/components/debug-monitor/DebugBattle.vue";

const { isOpen, healthStatus, lastErrorCode, lastErrorDetail, currentState, lastChangedAt } =
  useDebugMonitor();
</script>

<template>
  <aside
    v-if="isOpen"
    class="fixed top-0 right-0 z-1000 h-screen w-105 overflow-y-auto custom-scroll bg-black/85 border-l border-cyan-900 p-3 flex flex-col gap-3 font-mono text-[11px] leading-snug text-gray-200"
  >
    <DebugHeader
      :health-status="healthStatus"
      :last-error-code="lastErrorCode"
      :last-error-detail="lastErrorDetail"
      :last-changed-at="lastChangedAt"
    />

    <template v-if="currentState !== null">
      <DebugPlayer :player="currentState.player" />
      <DebugParty :party="currentState.party" :map-id="currentState.player.mapId" />
      <DebugBattle
        :digimon-battle="currentState.digimonBattle"
        :card-battle="currentState.cardBattle"
      />
    </template>
  </aside>
</template>
