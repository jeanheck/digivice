<script setup lang="ts">
import { computed, ref } from "vue";
import Modal from "@/components/modal/Modal.vue";
import EnemiesSection from "@/components/map/map-modal/EnemiesSection.vue";
import DocksSection from "@/components/map/map-modal/DocksSection.vue";
import BoxesSection from "@/components/map/map-modal/BoxesSection.vue";

type MapModalSection = "enemies" | "docks" | "boxes";

const props = defineProps<{
  isOpen: boolean;
  title: string;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const isModalOpen = computed(() => {
  return props.isOpen;
});

const selectedSection = ref<MapModalSection>("enemies");

const closeModal = () => {
  emit("close");
};

const selectSection = (section: MapModalSection) => {
  selectedSection.value = section;
};
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-450"
    max-height="h-[92vh] max-h-250"
    panel-class="w-[98vw]"
    @close="closeModal"
  >
    <template #header>
      <div class="flex items-center gap-6 flex-1 min-w-0">
        <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap shrink-0">
          {{ title }}
        </h2>

        <nav class="flex items-center gap-4 min-w-0">
          <button
            type="button"
            class="text-sm tracking-widest px-2 py-1 rounded border transition-colors duration-150 cursor-pointer"
            :class="selectedSection === 'enemies'
              ? 'text-red-600 border-red-600 bg-red-500/15'
              : 'text-red-900 border-transparent hover:text-red-600 hover:border-red-600 hover:bg-red-500/15'"
            @click="selectSection('enemies')"
          >
            {{ $t("map.enemies") }}
          </button>

          <button
            type="button"
            class="text-sm tracking-widest px-2 py-1 rounded border transition-colors duration-150 cursor-pointer"
            :class="selectedSection === 'docks'
              ? 'text-cyan-300 border-cyan-300 bg-cyan-300/15'
              : 'text-cyan-500 border-transparent hover:text-cyan-300 hover:border-cyan-300 hover:bg-cyan-300/15'"
            @click="selectSection('docks')"
          >
            {{ $t("map.docks") }}
          </button>

          <button
            type="button"
            class="text-sm tracking-widest px-2 py-1 rounded border transition-colors duration-150 cursor-pointer"
            :class="selectedSection === 'boxes'
              ? 'text-yellow-500 border-yellow-500 bg-yellow-500/15'
              : 'text-yellow-700 border-transparent hover:text-yellow-500 hover:border-yellow-500 hover:bg-yellow-500/15'"
            @click="selectSection('boxes')"
          >
            {{ $t("map.boxes") }}
          </button>
        </nav>
      </div>
    </template>

    <div class="flex flex-1 min-h-0 flex-col overflow-hidden">
      <EnemiesSection v-if="selectedSection === 'enemies'" />
      <DocksSection v-else-if="selectedSection === 'docks'" />
      <BoxesSection v-else-if="selectedSection === 'boxes'" />
    </div>
  </Modal>
</template>
