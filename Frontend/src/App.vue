<script setup lang="ts">
import { computed } from "vue";
import { useGameStore } from "@/stores/use-game-store";
import Journal from "@/components/journal/Journal.vue";
import Map from "@/components/map/Map.vue";
import Footer from "@/components/footer/Footer.vue";
import Digimon from "@/components/digimon/Digimon.vue";
import { AppPresenter } from "@/presenters/app.presenter";

const store = useGameStore();

const slotsWithDigimon = computed(() => {
  return AppPresenter.getFilledSlots(store.currentState?.party?.slots);
});
</script>

<template>
  <main class="min-h-screen bg-transparent p-4 flex flex-col gap-4 max-w-450 mx-auto text-white">
    <div class="flex-1 flex gap-4 min-h-150">
      <div class="flex-3 grid grid-cols-3 gap-4">
        <Digimon
          v-for="digimonSlot in slotsWithDigimon"
          :key="digimonSlot.index"
          :slot="digimonSlot"
        />
      </div>

      <div class="flex-1 min-w-75 min-h-0 overflow-hidden flex flex-col gap-4">
        <div class="flex-3 min-h-0 overflow-hidden flex flex-col">
          <Journal class="flex-1" />
        </div>

        <div class="flex-2 min-h-50 flex flex-col">
          <Map class="flex-1" />
        </div>
      </div>
    </div>

    <Footer />
  </main>
</template>
