<script setup lang="ts">
defineProps<{
  label: string;
  selected: boolean;
  activeClass: string;
  inactiveClass: string;
}>();

const emit = defineEmits<{
  (e: "click"): void;
}>();

const handleClick = () => {
  emit("click");
};
</script>

<template>
  <button
    type="button"
    class="text-sm tracking-widest px-2 py-1 rounded border transition-colors duration-150 cursor-pointer"
    :class="selected ? activeClass : inactiveClass"
    @click="handleClick"
  >
    {{ label }}
  </button>
</template>
