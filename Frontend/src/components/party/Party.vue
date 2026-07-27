<script setup lang="ts">
import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import Digimon from "@/components/party/digimon/Digimon.vue";
import { PartyPresenter } from "@/presenters/party/party.presenter";

const store = useGameStore();

const filledSlots = computed(() => {
  return PartyPresenter.getFilledSlots(store.currentState?.party?.slots);
});
</script>

<template>
  <div class="h-full min-h-0 grid grid-cols-3 gap-4">
    <Digimon
      v-for="digimonSlot in filledSlots"
      :key="digimonSlot.index"
      :slot="digimonSlot"
    />
  </div>
</template>
