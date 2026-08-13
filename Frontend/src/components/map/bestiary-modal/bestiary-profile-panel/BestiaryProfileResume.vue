<script setup lang="ts">
import { computed } from "vue";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = defineProps<{
  enemy: EnemyViewModel;
}>();

const emit = defineEmits<{
  (e: "open-locations"): void;
}>();

const hasLocations = computed(() => {
  return (props.enemy.locations?.length ?? 0) > 0;
});
</script>

<template>
  <div
    class="h-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-start gap-2.5 min-h-0"
  >
    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.specie") }}:</span
      >
      <span class="font-bold text-gray-300 capitalize">{{
        $t(`species.${enemy.species}`)
      }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.level") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.level }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase">HP:</span>
      <span class="font-bold text-white">{{ enemy.hp }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase">
        {{ $t("enemy.baseExp") }}:
      </span>
      <span class="font-bold text-gray-300">{{ enemy.exp }}</span>
    </div>

    <div class="flex items-center justify-between text-xs">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.baseBits") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.bits }}</span>
    </div>

    <div
      v-if="hasLocations"
      class="border-t border-blue-900/50 pt-2.5 flex flex-col gap-1.5 min-h-0 text-xs"
    >
      <span class="font-bold text-blue-500 tracking-wider uppercase">{{
        $t("enemy.foundIn")
      }}</span>
      <button
        type="button"
        class="self-start px-2.5 py-1.5 rounded text-[10px] font-bold uppercase tracking-widest text-blue-300 border border-blue-700/60 bg-blue-950/40 hover:bg-blue-900/40 transition-colors cursor-pointer"
        @click="emit('open-locations')"
      >
        {{ $t("enemy.locations") }}
      </button>
    </div>
  </div>
</template>
