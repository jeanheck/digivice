<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import { WikiNpcPanelPresenter } from "@/presenters/map/wiki-modal/wiki-npc-panel.presenter";
import WikiNpcCardBattlePanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcCardBattlePanel.vue";
import WikiNpcDigimonBattlePanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcDigimonBattlePanel.vue";

const props = defineProps<{
  npcId: string;
}>();

const battleOptions = computed(() => {
  return WikiNpcPanelPresenter.getBattleOptions(props.npcId);
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

const selectOption = (optionId: string) => {
  selectedOptionId.value = optionId;
};
</script>

<template>
  <div class="p-4 flex flex-col h-full min-h-0 overflow-hidden gap-3">
    <div
      v-if="battleOptions.length > 0"
      class="flex flex-wrap items-center gap-x-4 gap-y-1 shrink-0 border-b border-[#0055ff]/30 pb-2"
    >
      <button
        v-for="option in battleOptions"
        :key="option.id"
        type="button"
        class="font-bold text-[10px] 2xl:text-xs tracking-wide transition-colors cursor-pointer focus:outline-none"
        :class="
          selectedOptionId === option.id ? 'text-sky-300' : 'text-gray-400 hover:text-gray-300'
        "
        @click="selectOption(option.id)"
      >
        {{ $t(battleKindLabelKey(option.kind)) }} ({{ option.charismaRangeText }})
      </button>
    </div>

    <WikiNpcCardBattlePanel
      v-if="selectedOption?.kind === NpcBattleKindConstant.card"
      :npc-id="npcId"
      :battle-index="selectedOption.battleIndex"
    />
    <WikiNpcDigimonBattlePanel
      v-else-if="selectedOption?.kind === NpcBattleKindConstant.digimon"
      :npc-id="npcId"
      :battle-index="selectedOption.battleIndex"
    />
  </div>
</template>
