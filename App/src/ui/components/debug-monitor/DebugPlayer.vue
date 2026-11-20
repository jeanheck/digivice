<script setup lang="ts">
import type { Player } from "@/state/models";

const props = defineProps<{
  player: Player;
}>();
</script>

<template>
  <section class="flex flex-col gap-0.5">
    <div class="text-cyan-300 font-bold">PLAYER</div>
    <div><span class="text-gray-400">Bits: </span>{{ props.player.bits }}</div>
    <div>
      <span class="text-gray-400">Map: </span>{{ props.player.mapId }}
      <span class="text-gray-400"> | Previous: </span>{{ props.player.previousMapId }}
    </div>
    <div>
      <span class="text-gray-400">Seabed route: </span>{{ props.player.seabedRoute ?? "-" }}
      <span class="text-gray-400"> | Map variant: </span>{{ props.player.mapVariant ?? "-" }}
    </div>
  </section>
</template>
