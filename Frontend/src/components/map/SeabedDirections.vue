<script setup lang="ts">
import type { SeabedDirectionViewModel } from "@/viewmodels/map/seabed-direction.viewmodel";

const topPrimary: SeabedDirectionViewModel = {
  direction: "↖️",
  location: "Floresta dos Pontapés",
};

const topSecondary: SeabedDirectionViewModel | null = {
  direction: "⬆️",
  location: "Ponte do Pântano",
};

const middle: SeabedDirectionViewModel = {
  direction: "↖️",
  location: "Wire Forest Entrance",
};

const bottom: SeabedDirectionViewModel = {
  direction: "➡️",
  location: "Selva Sepultura",
};
</script>

<template>
  <div class="w-full flex justify-center shrink-0 px-0.5">
    <div class="seabed-directions">
      <div class="seabed-directions-post" aria-hidden="true" />
      <div class="seabed-directions-stack">
        <div class="seabed-direction">
          <div class="seabed-direction-inner">
            <template v-if="topSecondary === null">
              <span class="seabed-direction-emoji seabed-direction-emoji--left">{{ topPrimary.direction }}</span>
              <span class="seabed-direction-label">{{ topPrimary.location }}</span>
              <span class="seabed-direction-emoji seabed-direction-emoji--right">{{ topPrimary.direction }}</span>
            </template>
            <template v-else>
              <span class="seabed-direction-emoji seabed-direction-emoji--left">{{ topPrimary.direction }}</span>
              <div class="seabed-direction-split">
                <div class="seabed-direction-half seabed-direction-half--start">
                  <span class="seabed-direction-label">{{ topPrimary.location }}</span>
                </div>
                <div class="seabed-direction-half seabed-direction-half--end">
                  <span class="seabed-direction-label">{{ topSecondary.location }}</span>
                </div>
              </div>
              <span class="seabed-direction-emoji seabed-direction-emoji--right">{{ topSecondary.direction }}</span>
            </template>
          </div>
        </div>

        <div class="seabed-direction">
          <div class="seabed-direction-inner">
            <span class="seabed-direction-emoji seabed-direction-emoji--left">{{ middle.direction }}</span>
            <span class="seabed-direction-label">{{ middle.location }}</span>
          </div>
        </div>

        <div class="seabed-direction">
          <div class="seabed-direction-inner">
            <span class="seabed-direction-label">{{ bottom.location }}</span>
            <span class="seabed-direction-emoji seabed-direction-emoji--right">{{ bottom.direction }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
