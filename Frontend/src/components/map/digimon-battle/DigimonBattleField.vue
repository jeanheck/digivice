<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { DigimonBattleFieldPresenter } from "@/presenters/map/digimon-battle-field.presenter";

const props = defineProps<{
  battleFieldId: number;
}>();

const { t } = useI18n();

const digimonBattleFieldViewModel = computed(() => {
  return DigimonBattleFieldPresenter.getDigimonBattleFieldViewModel(props.battleFieldId, t);
});
</script>

<template>
  <div class="absolute bottom-2 left-2 z-5 flex flex-col gap-0.5 pointer-events-none">
    <span
      class="text-[10px] 2xl:text-sm font-bold tracking-wide text-white text-outline-black-glow leading-tight"
    >
      {{ digimonBattleFieldViewModel.title }}
    </span>
    <span
      v-if="digimonBattleFieldViewModel.strengthen"
      class="text-[10px] 2xl:text-sm font-bold tracking-wide text-green-400 text-outline-black-glow leading-tight"
    >
      {{ digimonBattleFieldViewModel.strengthen }}
    </span>
    <span
      v-if="digimonBattleFieldViewModel.weaken"
      class="text-[10px] 2xl:text-sm font-bold tracking-wide text-red-400 text-outline-black-glow leading-tight"
    >
      {{ digimonBattleFieldViewModel.weaken }}
    </span>
  </div>
</template>
