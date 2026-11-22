<script setup lang="ts">
defineProps<{
  locationId: string;
}>();

const emit = defineEmits<{
  (e: "open-location", locationId: string): void;
}>();

const handleSelect = (locationId: string): void => {
  emit("open-location", locationId);
};
</script>

<template>
  <section class="shrink-0 w-full bg-panel border border-frame/50 rounded p-4 shadow-inner">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-accent mb-3">
      {{ $t("enemy.locatedIn") }}
    </h4>

    <div class="flex flex-wrap gap-2">
      <button
        type="button"
        class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-accent/15 hover:bg-accent/25 border border-frame/50"
        @click="handleSelect(locationId)"
      >
        <span class="min-w-0">
          <span class="block text-xs font-bold text-ink tracking-wide">
            {{ $t(`map.${locationId}`) }}
          </span>
        </span>
      </button>
    </div>
  </section>
</template>
