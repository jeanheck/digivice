<script setup lang="ts">
import { computed, ref } from "vue";
import { useI18n } from "vue-i18n";
import Modal from "@/components/modal/Modal.vue";
import Sections from "@/components/map/map-modal/Sections.vue";
import EnemiesSection from "@/components/map/map-modal/EnemiesSection.vue";
import DocksSection from "@/components/map/map-modal/DocksSection.vue";
import BoxesSection from "@/components/map/map-modal/BoxesSection.vue";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

type MapModalSection = "enemies" | "docks" | "boxes";

const props = defineProps<{
  isOpen: boolean;
  location: LocationViewModel | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const { t } = useI18n();

const isModalOpen = computed(() => {
  return props.isOpen;
});

const locationName = computed(() => {
  return props.location?.id ? t(`location.${props.location.id}`) : t("map.unknownZone");
});

const enemyIds = computed(() => {
  return props.location?.enemies ?? [];
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
          {{ locationName }}
        </h2>

        <Sections :selected="selectedSection" @select="selectSection" />
      </div>
    </template>

    <div class="flex flex-1 min-h-0 flex-col overflow-hidden">
      <EnemiesSection
        v-if="selectedSection === 'enemies'"
        :enemy-ids="enemyIds"
      />
      <DocksSection v-else-if="selectedSection === 'docks'" />
      <BoxesSection v-else-if="selectedSection === 'boxes'" />
    </div>
  </Modal>
</template>
