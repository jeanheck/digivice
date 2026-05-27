<script setup lang="ts">
import { computed, ref } from "vue";
import { invoke } from "@tauri-apps/api/core";
import LanguageSelector from "@/components/footer/LanguageSelector.vue";
import DefaultTooltip from "@/components/tooltip/DefaultTooltip.vue";
import { useLocalization } from "@/composables/useLocalization";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { useGameStore } from "@/stores/use-game-store";
import { FooterPresenter } from "@/presenters/footer.presenter";

const store = useGameStore();
const { t } = useLocalization();

defineProps<{
  playerName: string;
  bits: number;
  isConnected: boolean;
}>();

const tooltipPosition = useTooltipPosition(300);
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = tooltipPosition;
const tooltipTitle = ref("");
const tooltipText = ref("");

const showGroupCharismaTooltip = (event: MouseEvent) => {
  tooltipTitle.value = t("party.groupCharisma");
  tooltipText.value = t("party.groupCharismaWarning");
  showAt(event, { maxWidth: 300, placement: "above" });
};

const moveGroupCharismaTooltip = (event: MouseEvent) => {
  move(event, "above");
};

const hideGroupCharismaTooltip = () => {
  hide();
};

const openLogsFolder = async () => {
  try {
    await invoke('open_logs_folder');
  } catch (err) {
    console.error('Failed to open logs folder:', err);
  }
};

const groupCharisma = computed(() => {
    return FooterPresenter.getPartyCharisma(store.currentState?.party?.slots ?? []);
});
</script>

<template>
  <footer class="w-full bg-[#000a2b] text-white p-3 rounded-md shadow-lg border-2 border-[#0033aa] border-t-orange-500 flex items-center gap-12 px-6 relative">
    <div class="font-bold text-lg">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">{{ $t('player.tamer') }}:</span>
      <span class="text-yellow-400 drop-shadow">{{ playerName }}</span>
    </div>
    
    <div class="font-bold text-lg flex items-baseline">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">{{ $t('player.bits') }}:</span>
      <span class="text-white">{{ bits }}</span>
    </div>
    
    <div class="font-bold text-lg flex items-baseline cursor-help"
      @mouseenter="showGroupCharismaTooltip"
      @mousemove="moveGroupCharismaTooltip"
      @mouseleave="hideGroupCharismaTooltip">
      <span class="opacity-80 text-[0.7rem] mr-2 font-normal text-blue-300 tracking-wider uppercase">{{ $t('party.groupCharisma') }}:</span>
      <span class="text-white">{{ groupCharisma }}</span>
    </div>

    <div class="absolute right-4 top-1/2 -translate-y-1/2 flex items-center gap-4 text-sm opacity-80">
      <div class="flex items-center gap-3 border-r border-blue-900 pr-4 mr-2">
        <button @click="openLogsFolder" class="px-2 py-1 text-xs bg-blue-900/50 hover:bg-blue-800 rounded border border-blue-700 transition-colors text-blue-200 hover:text-white uppercase tracking-wider">
          Logs
        </button>
        <LanguageSelector />
      </div>
      
      <div class="flex items-center gap-2">
        <span class="w-3 h-3 rounded-full" :class="isConnected ? 'bg-green-500' : 'bg-red-500'"></span>
        {{ isConnected ? $t('connection.connected') : $t('connection.disconnected') }}
      </div>
    </div>
  </footer>

  <DefaultTooltip
    :show="tooltipShow"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :text="tooltipText"
    placement="above"
  />
</template>
