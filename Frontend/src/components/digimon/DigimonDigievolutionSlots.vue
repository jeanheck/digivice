<script setup lang="ts">
import type { DigievolutionSlot } from "@/models";
import DigimonDigievolution from "@/components/digimon/DigimonDigievolution.vue";

const props = defineProps<{
  slots: DigievolutionSlot[];
  activeDigievolutionId: number | null;
}>();
</script>

<template>
  <div class="flex flex-col gap-0.5 w-full">
    <template v-for="slot in props.slots" :key="slot.index">
      <DigimonDigievolution
        v-if="slot"
        :digievolution-id="slot.digievolutionId!"
        :digievolution-level="slot.digievolution?.level!"
        :active-digievolution-id="activeDigievolutionId"
      />
      <div
        v-else
        class="evo-row relative flex w-full h-7 bg-[#000a2b] text-white overflow-hidden dw3-beveled cursor-default"
      >
        <div class="absolute inset-0 bg-[#0077ff] pointer-events-none dw3-beveled"></div>
        <div class="absolute inset-[1.5px] bg-[#000a2b] pointer-events-none dw3-beveled"></div>

        <div class="relative z-10 flex-1 flex items-center px-4 font-bold text-sm tracking-wider text-white/80 shadow-text">
          {{ $t("digimon.states.empty") }}
        </div>
      </div>
    </template>
  </div>
</template>
