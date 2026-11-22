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
    class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-accent/15 hover:bg-accent/25 border border-frame/50"
    @click="$emit('select')"
  >
    <span class="min-w-0">
      <span class="block text-xs font-bold text-ink tracking-wide">
        {{ $t(`boosters.${boosterId}.name`) }}
      </span>
    </span>
  </button>
</template>
