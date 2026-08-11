<script setup lang="ts">
const emit = defineEmits<{
  (e: "back"): void;
}>();
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-y-auto custom-scroll">
    <button
      type="button"
      class="self-start text-[10px] 2xl:text-xs font-bold uppercase tracking-widest text-blue-400 hover:text-blue-300 transition-colors cursor-pointer"
      @click="emit('back')"
    >
      {{ $t("enemy.backToEnemyInfo") }}
    </button>
  </div>
</template>
