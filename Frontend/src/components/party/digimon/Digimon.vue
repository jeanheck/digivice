<script setup lang="ts">
import { computed, ref } from "vue";
import Profile from "@/components/party/digimon/profile/Profile.vue";
import Digievolutions from "@/components/party/digimon/digievolutions/Digievolutions.vue";
import DigievolutionsModal from "@/components/party/digimon/digievolutions-modal/DigievolutionsModal.vue";
import Stats from "@/components/party/digimon/stats/Stats.vue";
import Equipments from "@/components/party/digimon/equipments/Equipments.vue";
import type { DigimonSlot } from "@/models";

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
  <div class="flex flex-col h-full w-full bg-[#000e3f] p-2 rounded-md shadow-lg border-2 border-[#0033aa] gap-2">
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

    <DigievolutionsModal
      :is-open="isGridModalOpen"
      :digimon="digimon"
      :digimon-id="digimonId"
      @close="closeDigievolutionGrid"
    />
  </div>
</template>
