<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import { WikiNpcPanelPresenter } from "@/presenters/map/wiki-modal/wiki-npc-panel.presenter";
import WikiNpcCardBattlePanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcCardBattlePanel.vue";
import WikiNpcDigimonBattlePanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcDigimonBattlePanel.vue";

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

const panelViewModel = computed(() => {
  return WikiNpcPanelPresenter.getPanelViewModel(props.npcId);
});

const battleOptions = computed(() => {
  return panelViewModel.value?.battleOptions ?? [];
});

const selectedOptionId = ref<string | null>(null);

watch(
  battleOptions,
  (options) => {
    selectedOptionId.value = options[0]?.id ?? null;
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

const npcTypeLabelKey = computed(() => {
  if (panelViewModel.value === null) {
    return "";
  }

  return `enemy.searchKind.${panelViewModel.value.type}`;
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
        class="aspect-square w-full flex items-center justify-center bg-[#000a1a] border border-blue-900/50 rounded text-2xl font-bold text-gray-500"
        aria-hidden="true"
      >
        ?
      </div>

      <span class="text-center text-xs font-bold text-gray-300 tracking-wide uppercase">
        {{ $t(npcTypeLabelKey) }}
      </span>

      <button
        type="button"
        class="text-center text-xs font-bold text-sky-300 hover:text-sky-200 tracking-wide transition-colors cursor-pointer focus:outline-none"
        @click="openLocation"
      >
        {{ $t(`location.${panelViewModel.locationId}`) }}
      </button>

      <div class="flex flex-col gap-2 pt-2 border-t border-[#0055ff]/30">
        <button
          v-for="option in battleOptions"
          :key="option.id"
          type="button"
          class="text-left font-bold text-[10px] 2xl:text-xs tracking-wide transition-colors cursor-pointer focus:outline-none px-1"
          :class="
            selectedOptionId === option.id ? 'text-sky-300' : 'text-gray-400 hover:text-gray-300'
          "
          @click="selectOption(option.id)"
        >
          {{ $t(battleKindLabelKey(option.kind)) }} ({{ option.charismaRangeText }})
        </button>
      </div>
    </aside>

    <div class="flex-1 min-w-0 min-h-0 flex flex-col overflow-hidden">
      <WikiNpcCardBattlePanel
        v-if="selectedOption?.kind === NpcBattleKindConstant.card"
        :npc-id="npcId"
        :battle-index="selectedOption.battleIndex"
        @open-card="emit('open-card', $event)"
        @open-drops="emit('open-drops', $event)"
      />
      <WikiNpcDigimonBattlePanel
        v-else-if="selectedOption?.kind === NpcBattleKindConstant.digimon"
        :npc-id="npcId"
        :battle-index="selectedOption.battleIndex"
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
