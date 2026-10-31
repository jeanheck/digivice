<script setup lang="ts">
import { computed } from "vue";
import { useGameState } from "@/ui/composables/use-game-state";
import Digimon from "@/ui/components/party/digimon/Digimon.vue";
import { PartyPresenter } from "@/presentation/presenters/party/party.presenter";

const gameState = useGameState();

const filledSlots = computed(() => {
  return PartyPresenter.getFilledSlots(gameState.value.party.slots);
});
</script>

<template>
  <div class="h-full min-h-0 grid grid-cols-3 gap-2">
    <Digimon v-for="digimonSlot in filledSlots" :key="digimonSlot.index" :slot="digimonSlot" />
  </div>
</template>
