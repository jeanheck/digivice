<script setup lang="ts">
import { computed, ref } from "vue";
import Profile from "@/ui/components/party/digimon/profile/Profile.vue";
import Digievolutions from "@/ui/components/party/digimon/digievolutions/Digievolutions.vue";
import DigievolutionsModal from "@/ui/components/party/digimon/digievolutions-modal/DigievolutionsModal.vue";
import Stats from "@/ui/components/party/digimon/stats/Stats.vue";
import Equipments from "@/ui/components/party/digimon/equipments/Equipments.vue";
import type { DigimonSlot } from "@/state/models";

const props = defineProps<{
  slot: DigimonSlot;
}>();

const digimon = computed(() => {
  return props.slot.digimon!;
});

const digimonId = computed(() => {
  return props.slot.digimonId!;
});

const isGridModalOpen = ref(false);

function openDigievolutionGrid(): void {
  isGridModalOpen.value = true;
}

function closeDigievolutionGrid(): void {
  isGridModalOpen.value = false;
}
</script>

<template>
  <div class="dw3-frame flex flex-col h-full min-h-0 w-full">
    <div
      class="flex flex-col flex-1 min-h-0 overflow-y-auto custom-scroll p-2 gap-2 min-[1600px]:p-3 min-[1600px]:gap-3 min-[1920px]:p-4 min-[1920px]:gap-4"
    >
      <Profile
        :digimon="digimon"
        :digimon-id="digimonId"
        @open-digievolutions="openDigievolutionGrid"
      />
      <Digievolutions
        :slots="digimon.digievolutions"
        :active-digievolution-id="digimon.activeDigievolutionId"
      />
      <Stats :digimon="digimon" />
      <Equipments :equipments="digimon.equipments" />
    </div>

    <DigievolutionsModal
      :is-open="isGridModalOpen"
      :digimon="digimon"
      :digimon-id="digimonId"
      @close="closeDigievolutionGrid"
    />
  </div>
</template>
