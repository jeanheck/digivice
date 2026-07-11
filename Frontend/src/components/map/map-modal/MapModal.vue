<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { useI18n } from "vue-i18n";
import Modal from "@/components/modal/Modal.vue";
import Sections from "@/components/map/map-modal/Sections.vue";
import EnemiesSection from "@/components/map/map-modal/EnemiesSection.vue";
import DocksSection from "@/components/map/map-modal/DocksSection.vue";
import type { LocationViewModel } from "@/viewmodels/location/location.viewmodel";

type MapModalSection = "enemies" | "docks";

const props = defineProps<{
  isOpen: boolean;
  location: LocationViewModel | null;
  initialEnemyId: string | null;
  initialSection: MapModalSection;
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
const selectedEnemyId = ref<string | null>(null);

const selectFirstEnemy = () => {
  selectedEnemyId.value = enemyIds.value[0] ?? null;
};

const applyOpenSelection = () => {
  selectedSection.value = props.initialSection;
  selectedEnemyId.value = props.initialEnemyId;
};

watch(
  () => props.isOpen,
  (isOpen) => {
    if (isOpen) {
      applyOpenSelection();
    }
  }
);

watch(
  enemyIds,
  () => {
    if (!props.isOpen || selectedSection.value !== "enemies") {
      return;
    }

    if (selectedEnemyId.value === null || !enemyIds.value.includes(selectedEnemyId.value)) {
      selectFirstEnemy();
    }
  }
);

const closeModal = () => {
  emit("close");
};

const selectSection = (section: MapModalSection) => {
  selectedSection.value = section;

  if (section === "enemies" && selectedEnemyId.value === null) {
    selectFirstEnemy();
  }
};

const selectEnemy = (enemyId: string) => {
  selectedEnemyId.value = enemyId;
};
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-[1334px]"
    max-height="h-[700px] max-h-[700px]"
    panel-class="w-[1334px]"
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
        :selected-enemy-id="selectedEnemyId"
        @select-enemy="selectEnemy"
      />
      <DocksSection v-else-if="selectedSection === 'docks'" :location="location" />
    </div>
  </Modal>
</template>
