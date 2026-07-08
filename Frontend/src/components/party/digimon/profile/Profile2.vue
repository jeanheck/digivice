<script setup lang="ts">
import ProgressBar from "@/components/party/digimon/profile/ProgressBar.vue";
import Icon from "@/components/party/digimon/profile/Icon.vue";
import { ProgressBarConstant } from "@/constants/progress-bar.constant";

const mockDigimonName = "Renamon";
const mockLevel = 8;
const mockExperience = { current: 10, max: 20, percentage: 50 };
const mockHp = { current: 20, max: 30, percentage: 67 };
const mockMp = { current: 40, max: 50, percentage: 80 };
const mockBlast = { current: 50, max: 60, percentage: 83 };
const mockTp = 10;
</script>

<template>
  <div class="dw3-panel flex flex-col">
    <div class="dw3-panel-border dw3-beveled"></div>
    <div class="dw3-panel-inner dw3-beveled"></div>

    <div class="dw3-panel-content p-2">
      <div class="grid grid-cols-[auto_1fr] grid-rows-5 gap-x-2 gap-y-1">
        <div class="col-start-1 row-start-1 row-span-3 w-20">
          <Icon :digimon-name="mockDigimonName" class="w-full aspect-square" />
        </div>

        <div
          class="col-start-1 row-start-4 w-20 h-6 bg-[#000e3f] border-2 border-[#00154a] rounded flex items-center justify-center gap-0.5 cursor-pointer hover:bg-[#001233] transition-colors"
        >
          <span class="text-[1.2rem] leading-none">🧬</span>
        </div>

        <div
          class="col-start-1 row-start-5 w-20 h-6 bg-[#000e3f] border-2 border-[#00154a] rounded flex items-center justify-center gap-0.5"
        >
          <span class="text-[1rem] leading-none">💪</span>
          <span class="text-[0.8rem] font-bold text-white">{{ mockTp }}</span>
        </div>

        <div class="col-start-2 row-start-1 flex justify-between items-baseline min-w-0 h-6">
          <h2 class="text-sm font-bold text-white leading-none truncate pr-2 tracking-wide">{{ mockDigimonName }}</h2>
          <span class="text-[0.6rem] font-medium text-yellow-400 shrink-0">
            Nv {{ mockLevel }}
          </span>
        </div>

        <div class="col-start-2 row-start-2 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.experience"
            :current-value="mockExperience.current"
            :max-value="mockExperience.max"
            :percentage="mockExperience.percentage"
          />
        </div>

        <div class="col-start-2 row-start-3 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.hp"
            :current-value="mockHp.current"
            :max-value="mockHp.max"
            :percentage="mockHp.percentage"
          />
        </div>

        <div class="col-start-2 row-start-4 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.mp"
            :current-value="mockMp.current"
            :max-value="mockMp.max"
            :percentage="mockMp.percentage"
          />
        </div>

        <div class="col-start-2 row-start-5 min-w-0 h-6">
          <ProgressBar
            :variant="ProgressBarConstant.blast"
            :current-value="mockBlast.current"
            :max-value="mockBlast.max"
            :percentage="mockBlast.percentage"
          />
        </div>
      </div>
    </div>
  </div>
</template>
