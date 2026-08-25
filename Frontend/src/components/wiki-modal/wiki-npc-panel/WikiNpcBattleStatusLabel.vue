<script setup lang="ts">
import { computed } from "vue";
import type { NpcBattleStatus } from "@/services/npc.service";

const props = defineProps<{
  battleStatus: NpcBattleStatus;
}>();

const battleStatusLabelKey = computed(() => {
  if (props.battleStatus === "completed") {
    return "npc.battle.status.completed";
  }

  if (props.battleStatus === "available") {
    return "npc.battle.status.available";
  }

  return "npc.battle.status.missingCharisma";
});

const battleStatusClass = computed(() => {
  if (props.battleStatus === "completed") {
    return "text-green-400";
  }

  if (props.battleStatus === "available") {
    return "text-cyan-400";
  }

  return "text-gray-400";
});
</script>

<template>
  <span
    class="font-bold tracking-wider uppercase text-xs whitespace-nowrap"
    :class="battleStatusClass"
  >
    {{ $t(battleStatusLabelKey) }}
  </span>
</template>
