<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { WikiNpcDigimonBattlePresenter } from "@/presenters/map/wiki-modal/wiki-npc-digimon-battle.presenter";
import WikiNpcDigimonBattleRewards from "@/components/wiki-modal/wiki-npc-panel/WikiNpcDigimonBattleRewards.vue";
import WikiNpcDigimonPortrait from "@/components/wiki-modal/wiki-npc-panel/WikiNpcDigimonPortrait.vue";
import WikiProfileAttributes from "@/components/wiki-modal/wiki-profile-panel/WikiProfileAttributes.vue";
import WikiProfileElements from "@/components/wiki-modal/wiki-profile-panel/WikiProfileElements.vue";
import WikiProfileConditions from "@/components/wiki-modal/wiki-profile-panel/WikiProfileConditions.vue";
import WikiProfileTechniques from "@/components/wiki-modal/wiki-profile-panel/WikiProfileTechniques.vue";
import WikiProfileDrops from "@/components/wiki-modal/wiki-profile-panel/WikiProfileDrops.vue";

const props = defineProps<{
  npcId: string;
  battleId: string;
}>();

const emit = defineEmits<{
  (e: "open-drops", dropId: string): void;
  (e: "show-stat-key-tooltip", event: MouseEvent, statKey: string): void;
  (e: "show-condition-tooltip", event: MouseEvent, tooltipKey: string): void;
  (e: "move-stat-tooltip", event: MouseEvent): void;
  (e: "hide-stat-tooltip"): void;
}>();

const battleViewModel = computed(() => {
  return WikiNpcDigimonBattlePresenter.getBattleViewModel(props.npcId, props.battleId);
});

const currentMemberIndex = ref(0);

watch(
  battleViewModel,
  () => {
    currentMemberIndex.value = 0;
  },
  { immediate: true },
);

const activeMember = computed(() => {
  if (battleViewModel.value === null) {
    return null;
  }

  return battleViewModel.value.members[currentMemberIndex.value] ?? null;
});
</script>

<template>
  <div
    v-if="battleViewModel !== null"
    class="flex-1 min-h-0 flex flex-col gap-3 p-3 overflow-hidden"
  >
    <WikiNpcDigimonBattleRewards
      v-model="currentMemberIndex"
      :exp="battleViewModel.exp"
      :dvexp="battleViewModel.dvexp"
      :bits="battleViewModel.bits"
      :member-count="battleViewModel.partyMemberCount"
    />

    <div
      v-if="activeMember !== null"
      class="flex-1 min-h-0 overflow-y-auto custom-scroll flex flex-col gap-3"
    >
      <div class="flex w-full gap-3 items-stretch flex-1 min-h-0">
        <WikiNpcDigimonPortrait
          class="w-[30%] min-w-0"
          :enemy="activeMember.enemy"
          :image-url="activeMember.imageUrl"
        />

        <div
          class="w-[70%] min-w-0 h-full bg-[#000a1a] border border-blue-900/50 rounded p-2 shadow-inner flex flex-row justify-around gap-2 items-start overflow-y-auto custom-scroll"
        >
          <WikiProfileAttributes
            :attributes="activeMember.enemy.attributes"
            @show-stat-key-tooltip="
              (event, statKey) => emit('show-stat-key-tooltip', event, statKey)
            "
            @move-stat-tooltip="emit('move-stat-tooltip', $event)"
            @hide-stat-tooltip="emit('hide-stat-tooltip')"
          />
          <WikiProfileElements
            :elements="activeMember.enemy.elements"
            @show-stat-key-tooltip="
              (event, statKey) => emit('show-stat-key-tooltip', event, statKey)
            "
            @move-stat-tooltip="emit('move-stat-tooltip', $event)"
            @hide-stat-tooltip="emit('hide-stat-tooltip')"
          />
          <WikiProfileConditions
            :conditions="activeMember.enemy.conditions"
            @show-condition-tooltip="
              (event, tooltipKey) => emit('show-condition-tooltip', event, tooltipKey)
            "
            @move-stat-tooltip="emit('move-stat-tooltip', $event)"
            @hide-stat-tooltip="emit('hide-stat-tooltip')"
          />
        </div>
      </div>

      <div class="grid grid-cols-2 gap-3 shrink-0">
        <WikiProfileTechniques :enemy="activeMember.enemy" />
        <WikiProfileDrops
          :drops="activeMember.enemy.drops"
          @open-drops="emit('open-drops', $event)"
        />
      </div>
    </div>
  </div>
</template>
