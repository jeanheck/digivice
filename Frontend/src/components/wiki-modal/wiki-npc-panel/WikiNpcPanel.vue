<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import { NpcTypeConstant } from "@/constants/npc-type.constant";
import { FooterPresenter } from "@/presenters/footer/footer.presenter";
import { WikiNpcPanelPresenter } from "@/presenters/map/wiki-modal/wiki-npc-panel.presenter";
import WikiNpcCardBattlePanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcCardBattlePanel.vue";
import WikiNpcDigimonBattlePanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcDigimonBattlePanel.vue";
import { QuestRepository } from "@/repositories/quest.repository";
import { QuestService } from "@/services/quest.service";
import { useGameStore } from "@/stores/use-game-store";
import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";

const props = defineProps<{
  npcId: string;
}>();

const emit = defineEmits<{
  (e: "open-locations", locationId: string): void;
  (e: "open-drops", dropId: string): void;
  (e: "open-card", cardId: string): void;
  (e: "show-stat-key-tooltip", event: MouseEvent, statKey: string): void;
  (e: "show-condition-tooltip", event: MouseEvent, tooltipKey: string): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const store = useGameStore();

const journalNpc = computed(() => {
  return (
    store.currentState?.journal?.npcs.find((npc) => {
      return npc.id === props.npcId;
    }) ?? null
  );
});

const partyCharisma = computed(() => {
  return FooterPresenter.getPartyCharisma(store.currentState?.party?.slots ?? []);
});

const importantItems = computed(() => {
  return store.currentState?.importantItems ?? null;
});

const panelViewModel = computed(() => {
  return WikiNpcPanelPresenter.getPanelViewModel(
    props.npcId,
    journalNpc.value,
    partyCharisma.value,
    importantItems.value,
  );
});

const battleOptions = computed(() => {
  return panelViewModel.value?.battleOptions ?? [];
});

const selectedOptionId = ref<string | null>(null);

watch(
  battleOptions,
  (options) => {
    selectedOptionId.value = WikiNpcPanelPresenter.getDefaultSelectedBattleOptionId(options);
  },
  { immediate: true },
);

const selectedOption = computed(() => {
  return (
    battleOptions.value.find((option) => {
      return option.id === selectedOptionId.value;
    }) ?? null
  );
});

const battleKindLabelKey = (kind: NpcBattleKindConstant): string => {
  if (kind === NpcBattleKindConstant.card) {
    return "npc.battle.card";
  }

  return "npc.battle.digimon";
};

const battleOptionClass = (option: WikiNpcBattleOptionViewModel): string => {
  const isSelected = selectedOptionId.value === option.id;

  if (option.status === "completed") {
    if (isSelected) {
      return "text-green-300 line-through decoration-green-400 border-green-500/60 bg-green-500/20 hover:bg-green-500/30";
    }

    return "text-green-400/80 line-through decoration-green-500 border-green-500/40 bg-green-500/10 hover:bg-green-500/20";
  }

  if (option.status === "available") {
    if (isSelected) {
      return "text-cyan-300 border-cyan-500/60 bg-cyan-900/30 hover:bg-cyan-900/50";
    }

    return "text-cyan-300 border-cyan-700/60 bg-cyan-950/40 hover:bg-cyan-900/60";
  }

  if (isSelected) {
    return "text-gray-200 border-gray-500 bg-gray-800 hover:bg-gray-800";
  }

  return "text-gray-500 border-gray-700/60 bg-gray-950/40 hover:bg-gray-900/60 hover:text-gray-300";
};

const showBattleRequirementTooltip = (
  event: MouseEvent,
  option: WikiNpcBattleOptionViewModel,
): void => {
  if (option.battleTooltipKey === null) {
    return;
  }

  emit("show-condition-tooltip", event, option.battleTooltipKey);
};

const npcTypeLabelKey = computed(() => {
  if (panelViewModel.value === null) {
    return "";
  }

  return `enemy.searchKind.${panelViewModel.value.type}`;
});

const showFolderBagRequirementHint = computed(() => {
  if (panelViewModel.value?.type !== NpcTypeConstant.tamer) {
    return false;
  }

  const folderBagQuest = store.currentState?.journal?.sideQuests.find((quest) => {
    return quest.id === "folderBag";
  });
  const folderBagRaw = QuestRepository.getSideQuestsRaw().find((questRaw) => {
    return questRaw.id === "folderBag";
  });

  if (folderBagRaw === undefined) {
    return false;
  }

  return !QuestService.isQuestCompleted(folderBagQuest, folderBagRaw);
});

const selectOption = (optionId: string) => {
  selectedOptionId.value = optionId;
};

const openLocation = () => {
  if (panelViewModel.value === null) {
    return;
  }

  emit("open-locations", panelViewModel.value.locationId);
};
</script>

<template>
  <div
    v-if="panelViewModel !== null"
    class="flex h-full min-h-0 overflow-hidden"
  >
    <aside
      class="w-[20%] shrink-0 flex flex-col gap-3 p-3 border-r border-[#0055ff]/30 min-h-0 overflow-y-auto custom-scroll"
    >
      <div
        class="aspect-square w-full flex items-center justify-center bg-[#000a1a] border border-blue-900/50 rounded overflow-hidden"
      >
        <img
          v-if="panelViewModel.imageUrl"
          :src="panelViewModel.imageUrl"
          :alt="panelViewModel.name"
          class="h-[90%] drop-shadow-[0_0_15px_rgba(0,170,255,0.2)]"
        />
        <span
          v-else
          class="text-2xl font-bold text-gray-500"
          aria-hidden="true"
        >
          ?
        </span>
      </div>

      <span class="text-center text-xs font-bold text-gray-300 tracking-wide uppercase">
        {{ $t(npcTypeLabelKey) }}
      </span>

      <button
        type="button"
        class="w-full text-center px-2.5 py-2 rounded text-[10px] 2xl:text-[12px] font-bold tracking-wide transition-colors cursor-pointer focus:outline-none hover:bg-blue-900/60 text-blue-300 border border-blue-700/60 bg-blue-950/40"
        @click="openLocation"
      >
        {{ $t(`location.${panelViewModel.locationId}`) }}
      </button>

      <div class="flex flex-col gap-2 pt-2 border-t border-[#0055ff]/30">
        <button
          v-for="option in battleOptions"
          :key="option.id"
          type="button"
          class="w-full text-center px-2.5 py-2 rounded text-[10px] font-bold tracking-wide transition-colors cursor-pointer focus:outline-none border"
          :class="battleOptionClass(option)"
          @click="selectOption(option.id)"
          @mouseenter="showBattleRequirementTooltip($event, option)"
          @mousemove="emit('move-stat-tooltip', $event)"
          @mouseleave="emit('hide-stat-tooltip')"
        >
          <span class="inline-flex items-center justify-center gap-1">
            <span>
              {{ $t(battleKindLabelKey(option.kind)) }} ({{ option.charismaRangeText }})
            </span>
            <span
              v-if="option.showAsukaTrophyEmoji"
              class="inline-flex leading-none text-[1.2rem] -translate-y-1"
              :class="{ grayscale: !option.asukaTrophyOwned }"
            >
              🏆
            </span>
          </span>
        </button>
      </div>

      <p
        v-if="showFolderBagRequirementHint"
        class="text-[10px] 2xl:text-[10px] text-red-400 leading-tight"
      >
        {{ $t("npc.folderBagRequirementHint") }}
      </p>
    </aside>

    <div class="flex-1 min-w-0 min-h-0 flex flex-col overflow-hidden">
      <WikiNpcCardBattlePanel
        v-if="selectedOption?.kind === NpcBattleKindConstant.card"
        :npc-id="npcId"
        :battle-id="selectedOption.battleId"
        @open-card="emit('open-card', $event)"
        @open-drops="emit('open-drops', $event)"
      />
      <WikiNpcDigimonBattlePanel
        v-else-if="selectedOption?.kind === NpcBattleKindConstant.digimon"
        :npc-id="npcId"
        :battle-id="selectedOption.battleId"
        @open-drops="emit('open-drops', $event)"
        @show-stat-key-tooltip="
          (event, statKey) => emit('show-stat-key-tooltip', event, statKey)
        "
        @show-condition-tooltip="
          (event, tooltipKey) => emit('show-condition-tooltip', event, tooltipKey)
        "
        @move-stat-tooltip="emit('move-stat-tooltip', $event)"
        @hide-stat-tooltip="emit('hide-stat-tooltip')"
      />
    </div>
  </div>
</template>
