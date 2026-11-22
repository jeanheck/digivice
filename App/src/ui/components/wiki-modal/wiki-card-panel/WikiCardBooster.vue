<script setup lang="ts">
defineProps<{
  boosterId: number;
}>();

defineEmits<{
  select: [];
}>();
</script>

<template>
  <button
    type="button"
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 in-[[data-theme=seabed]]:bg-[var(--color-accent)]/15 hover:bg-blue-900/50 in-[[data-theme=seabed]]:hover:bg-[var(--color-accent)]/25 border border-blue-800/60 in-[[data-theme=seabed]]:border-[var(--color-frame)]/50"
    @click="$emit('select')"
  >
    <span class="min-w-0">
      <span class="block text-xs font-bold text-blue-200 in-[[data-theme=seabed]]:text-[var(--color-ink)] tracking-wide">
        {{ $t(`boosters.${boosterId}.name`) }}
      </span>
    </span>
  </button>
</template>
