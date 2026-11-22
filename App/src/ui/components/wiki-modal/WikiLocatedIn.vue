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
  <section class="shrink-0 w-full bg-[#000a1a] in-[[data-theme=seabed]]:bg-[var(--color-surface-deep)] border border-blue-900/50 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50 rounded p-4 shadow-inner">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 in-[[data-theme=seabed]]:text-[var(--color-accent)] mb-3">
      {{ $t("enemy.locatedIn") }}
    </h4>

    <div class="flex flex-wrap gap-2">
      <button
        type="button"
        class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 in-[[data-theme=seabed]]:bg-[var(--color-accent)]/15 hover:bg-blue-900/50 in-[[data-theme=seabed]]:hover:bg-[var(--color-accent)]/25 border border-blue-800/60 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50"
        @click="handleSelect(locationId)"
      >
        <span class="min-w-0">
          <span class="block text-xs font-bold text-blue-200 in-[[data-theme=seabed]]:text-[var(--color-ink)] tracking-wide">
            {{ $t(`map.${locationId}`) }}
          </span>
        </span>
      </button>
    </div>
  </section>
</template>
