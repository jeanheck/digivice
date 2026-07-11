<script setup lang="ts">
import Section from "@/components/map/map-modal/Section.vue";

type MapModalSection = "enemies" | "docks";

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
      active-class="text-cyan-300 border-cyan-300 bg-cyan-300/15"
      inactive-class="text-[#0077ff] border-transparent hover:text-cyan-300 hover:border-cyan-300 hover:bg-cyan-300/15"
      @click="selectSection('enemies')"
    />

    <Section
      :label="$t('mapModal.docks')"
      :selected="selected === 'docks'"
      active-class="text-cyan-300 border-cyan-300 bg-cyan-300/15"
      inactive-class="text-[#0077ff] border-transparent hover:text-cyan-300 hover:border-cyan-300 hover:bg-cyan-300/15"
      @click="selectSection('docks')"
    />
  </nav>
</template>
