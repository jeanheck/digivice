<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import type { WikiDroppedBySourceViewModel } from "@/viewmodels/wiki-modal/wiki-dropped-by-source.viewmodel";

const props = defineProps<{
  source: WikiDroppedBySourceViewModel;
}>();

defineEmits<{
  select: [];
}>();

const { t } = useI18n();

const displayName = computed(() => {
  if (props.source.labelKey !== undefined) {
    return t(props.source.labelKey);
  }

  return props.source.label ?? "";
});

const locationLabel = (locationId: string): string => {
  return t("enemy.locationOnly", { location: t(`location.${locationId}`) });
};
</script>

<template>
  <button
    type="button"
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="$emit('select')"
  >
    <img
      v-if="source.iconUrl"
      :src="source.iconUrl"
      :alt="displayName"
      class="w-8 h-8 object-contain rendering-pixelated"
    />
    <span class="min-w-0">
      <span class="block text-xs font-bold text-blue-200 tracking-wide">
        {{ displayName }}
      </span>
      <span class="block min-h-3 text-[10px] text-gray-400 leading-tight">
        {{ source.kind === "enemy" && source.locationId ? locationLabel(source.locationId) : "" }}
      </span>
    </span>
  </button>
</template>
