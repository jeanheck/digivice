<script setup lang="ts">
import { computed } from "vue";
import WikiProfileLocations from "@/components/wiki-modal/wiki-profile-panel/WikiProfileLocations.vue";
import { WikiLocationsPanelPresenter } from "@/presenters/map/wiki-modal/wiki-locations-panel.presenter";
import { useGameStore } from "@/stores/use-game-store";
import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

const props = withDefaults(
  defineProps<{
    enemy: EnemyViewModel;
    showRewards?: boolean;
    showLocations?: boolean;
  }>(),
  {
    showRewards: true,
    showLocations: true,
  },
);

const emit = defineEmits<{
  (e: "open-locations", locationId: string): void;
}>();

const store = useGameStore();

const mainQuest = computed(() => {
  return store.currentState?.journal?.mainQuest ?? null;
});

const resolvedLocations = computed(() => {
  return WikiLocationsPanelPresenter.getResolvedEnemyLocations(
    props.enemy.locations,
    mainQuest.value,
  );
});

const hasResolvedLocations = computed(() => {
  return resolvedLocations.value.length > 0;
});

const showBaseExp = computed(() => {
  return props.showRewards && props.enemy.exp !== undefined;
});

const showBaseBits = computed(() => {
  return props.showRewards && props.enemy.bits !== undefined;
});

const handleOpenLocation = (locationId: string): void => {
  emit("open-locations", locationId);
};
</script>

<template>
  <div
    class="h-full bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col justify-start gap-2.5 min-h-0"
  >
    <div class="flex items-center justify-between text-xs shrink-0">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.specie") }}:</span
      >
      <span class="font-bold text-gray-300 capitalize">{{
        $t(`species.${enemy.species}`)
      }}</span>
    </div>

    <div class="flex items-center justify-between text-xs shrink-0">
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.level") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.level }}</span>
    </div>

    <div class="flex items-center justify-between text-xs shrink-0">
      <span class="font-bold text-blue-500 tracking-wider uppercase">HP:</span>
      <span class="font-bold text-white">{{ enemy.hp }}</span>
    </div>

    <div
      v-if="showBaseExp"
      class="flex items-center justify-between text-xs shrink-0"
    >
      <span class="font-bold text-blue-500 tracking-wider uppercase">
        {{ $t("enemy.baseExp") }}:
      </span>
      <span class="font-bold text-gray-300">{{ enemy.exp }}</span>
    </div>

    <div
      v-if="showBaseBits"
      class="flex items-center justify-between text-xs shrink-0"
    >
      <span class="font-bold text-blue-500 tracking-wider uppercase"
        >{{ $t("enemy.baseBits") }}:</span
      >
      <span class="font-bold text-gray-300">{{ enemy.bits }}</span>
    </div>

    <div
      v-if="showLocations"
      class="border-t border-blue-900/50 pt-2.5 flex flex-col flex-1 min-h-0 gap-1.5 text-xs"
    >
      <span
        v-if="hasResolvedLocations"
        class="text-center font-bold text-blue-500 tracking-wider uppercase shrink-0"
      >
        {{ $t("enemy.whereToFindLabel") }}
      </span>

      <WikiProfileLocations
        v-if="hasResolvedLocations"
        class="flex-1 min-h-0"
        :enemy="enemy"
        @open-location="handleOpenLocation"
      />

      <p
        v-else
        class="text-[12px] 2xl:text-[12px] text-red-400 leading-tight text-center flex-1 flex items-center justify-center"
      >
        {{ $t("enemy.unavailableAtMomentHint") }}
      </p>
    </div>
  </div>
</template>
