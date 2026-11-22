<script setup lang="ts">
import WikiCardBooster from "@/ui/components/wiki-modal/wiki-card-panel/WikiCardBooster.vue";

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
  <section class="shrink-0 w-1/2 bg-panel border border-frame/50 rounded p-4 shadow-inner">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-accent mb-3">
      {{ $t("enemy.droppedBy") }}
    </h4>

    <p v-if="boosterIds.length === 0" class="text-xs text-ink/70 italic">
      {{ $t("enemy.obtainedFromNone") }}
    </p>
    <div v-else class="flex flex-wrap gap-2">
      <WikiCardBooster
        v-for="boosterId in boosterIds"
        :key="boosterId"
        :booster-id="boosterId"
        @select="handleSelect(boosterId)"
      />
    </div>
  </section>
</template>
