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
  <section
    class="shrink-0 w-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner"
  >
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
      {{ $t("enemy.locatedIn") }}
    </h4>

    <div class="flex flex-wrap gap-2">
      <button
        type="button"
        class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
        @click="handleSelect(locationId)"
      >
        <span class="min-w-0">
          <span class="block text-xs font-bold text-blue-200 tracking-wide">
            {{ $t(`location.${locationId}`) }}
          </span>
        </span>
      </button>
    </div>
  </section>
</template>
