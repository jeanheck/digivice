<script setup lang="ts">
import type { WikiWeaponShopItemViewModel } from "@/presentation/viewmodels/wiki-modal/wiki-weapon-shop-item.viewmodel";

const props = defineProps<{
  item: WikiWeaponShopItemViewModel;
}>();

const emit = defineEmits<{
  (e: "select", equipmentId: string): void;
}>();

const handleSelect = (): void => {
  emit("select", props.item.equipmentId);
};
</script>

<template>
  <button
    type="button"
    class="flex items-start gap-3 w-full px-3 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
    @click="handleSelect"
  >
    <span class="font-emoji-color text-2xl leading-none shrink-0">&#x2694;</span>
    <span class="flex flex-col gap-1 flex-1 min-w-0">
      <span class="flex items-start justify-between gap-2">
        <span class="flex items-baseline gap-2 min-w-0">
          <span class="text-xs font-bold text-white tracking-wide truncate">
            {{ $t(`equipments.${item.equipmentId}.name`) }}
          </span>
          <span class="shrink-0 text-[8px] font-bold uppercase tracking-wide text-blue-400">
            {{ $t(`equipmentType.${item.equipment.type}`) }}
          </span>
        </span>
        <span class="flex items-baseline gap-1 shrink-0 text-[10px] font-bold whitespace-nowrap">
          <span class="text-amber-400">{{ item.price }}</span>
          <span class="text-white">{{ $t("enemy.bits") }}</span>
        </span>
      </span>
      <span class="text-[8px] text-blue-300/80 leading-tight line-clamp-2">
        {{ $t(`equipments.${item.equipmentId}.note`) }}
      </span>
      <span class="grid grid-cols-2 gap-2 items-start">
        <span class="flex flex-wrap items-center gap-x-3 gap-y-1 text-[10px] font-bold">
          <span
            v-for="attribute in item.attributes"
            :key="attribute.attribute"
            class="flex items-center gap-1"
          >
            <img
              v-if="attribute.iconUrl"
              :src="attribute.iconUrl"
              :alt="$t(`stat.${attribute.attribute}`)"
              class="w-[25px] h-[27px] object-contain rendering-pixelated"
            />
            <span :class="attribute.type === '+' ? 'text-green-400' : 'text-red-400'">
              {{ attribute.type }}{{ attribute.value }}
            </span>
          </span>
        </span>
        <span class="flex flex-col items-center gap-1 text-[10px] leading-tight text-center">
          <span class="text-blue-500 font-bold uppercase">{{ $t("digimon.equipableBy") }}:</span>
          <span class="text-white">
            <template v-if="item.equipment.equipableDigimonIds.length === 0">
              {{ $t("digimon.states.none") }}
            </template>
            <template v-else-if="item.equipment.equipableDigimonIds.length === 8">
              <span class="text-purple-300 font-bold uppercase">{{ $t("digimon.allDigimon") }}</span>
            </template>
            <template v-else>
              {{ item.equipment.equipableDigimonNames.join(", ") }}
            </template>
          </span>
        </span>
      </span>
    </span>
  </button>
</template>
