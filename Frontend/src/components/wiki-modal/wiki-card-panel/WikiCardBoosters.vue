<script setup lang="ts">
import WikiCardBooster from "@/components/wiki-modal/wiki-card-panel/WikiCardBooster.vue";

defineProps<{
  boosterIds: number[];
}>();

const emit = defineEmits<{
  (e: "open-drop", dropKey: string): void;
}>();

const handleSelect = (boosterId: number): void => {
  emit("open-drop", String(boosterId));
};
</script>

<template>
  <section
    class="shrink-0 w-1/2 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
  >
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
      {{ $t("enemy.obtainedFrom") }}
    </h4>

    <p
      v-if="boosterIds.length === 0"
      class="text-xs text-gray-400 italic"
    >
      {{ $t("enemy.obtainedFromNone") }}
    </p>
    <div
      v-else
      class="flex flex-wrap gap-2"
    >
      <WikiCardBooster
        v-for="boosterId in boosterIds"
        :key="boosterId"
        :booster-id="boosterId"
        @select="handleSelect(boosterId)"
      />
    </div>
  </section>
</template>
