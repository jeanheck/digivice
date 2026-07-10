<script setup lang="ts">
import Section from "@/components/map/map-modal/Section.vue";

type MapModalSection = "enemies" | "docks" | "boxes";

defineProps<{
  selected: MapModalSection;
}>();

const emit = defineEmits<{
  (e: "select", section: MapModalSection): void;
}>();

const selectSection = (section: MapModalSection) => {
  emit("select", section);
};
</script>

<template>
  <nav class="flex items-center gap-4 min-w-0">
    <Section
      :label="$t('mapModal.enemies')"
      :selected="selected === 'enemies'"
      active-class="text-[#bc3737] border-[#bc3737] bg-red-500/15"
      inactive-class="text-red-900 border-transparent hover:text-[#bc3737] hover:border-[#bc3737] hover:bg-red-500/15"
      @click="selectSection('enemies')"
    />

    <Section
      :label="$t('mapModal.docks')"
      :selected="selected === 'docks'"
      active-class="text-cyan-300 border-cyan-300 bg-cyan-300/15"
      inactive-class="text-cyan-500 border-transparent hover:text-cyan-300 hover:border-cyan-300 hover:bg-cyan-300/15"
      @click="selectSection('docks')"
    />

    <Section
      :label="$t('mapModal.boxes')"
      :selected="selected === 'boxes'"
      active-class="text-yellow-500 border-yellow-500 bg-yellow-500/15"
      inactive-class="text-yellow-700 border-transparent hover:text-yellow-500 hover:border-yellow-500 hover:bg-yellow-500/15"
      @click="selectSection('boxes')"
    />
  </nav>
</template>
