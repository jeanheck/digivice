<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import { WikiDropEquipmentPresenter } from "@/presenters/map/wiki-modal/wiki-drop-equipment.presenter";

const props = defineProps<{
  equipmentId: number;
}>();

const { t, te } = useI18n();

const equipment = computed(() => {
  return WikiDropEquipmentPresenter.getViewModel(props.equipmentId);
});

const equipmentName = computed(() => {
  return t(`equipments.${equipment.value.id}.name`);
});

const equipmentType = computed(() => {
  return t(`equipmentType.${equipment.value.type}`);
});

const equipmentNote = computed(() => {
  const noteKey = `equipments.${equipment.value.id}.note`;
  if (!te(noteKey)) {
    return "";
  }

  const translatedNote = t(noteKey);
  if (translatedNote === "" || translatedNote === noteKey) {
    return "";
  }

  return translatedNote;
});
</script>

<template>
  <div class="flex flex-col gap-2 min-h-0 overflow-y-auto custom-scroll">
    <h5 class="text-sm font-bold text-blue-200 tracking-wide">
      {{ equipmentName }}
    </h5>

    <div
      v-if="equipmentType"
      class="text-blue-300 text-[10px] tracking-widest uppercase font-bold"
    >
      {{ equipmentType }}
    </div>

    <div v-if="equipment.attributes.length > 0" class="flex flex-col gap-0.5">
      <div
        v-for="attribute in equipment.attributes"
        :key="attribute.attribute"
        class="flex justify-between text-xs items-center bg-[#002266]/40 px-1 rounded-sm"
      >
        <span class="text-gray-200">{{ t("stat." + attribute.attribute.toLowerCase()) }}</span>
        <span
          :class="attribute.type === '+' ? 'text-green-400' : 'text-red-400'"
          class="font-bold tracking-wider"
        >
          {{ attribute.type }}{{ attribute.value }}
        </span>
      </div>
    </div>

    <div class="pt-1 border-t border-[#0033aa]/50 flex flex-col gap-1">
      <span class="text-gray-400 text-[9px] uppercase tracking-widest leading-none">
        {{ $t("digimon.equipableBy") }}
      </span>
      <span class="text-gray-200 text-[11px] leading-tight">
        <template v-if="equipment.equipableDigimonIds.length === 0">
          {{ $t("digimon.states.none") }}
        </template>
        <template v-else-if="equipment.equipableDigimonIds.length === 8">
          <span class="text-purple-300 font-bold uppercase tracking-wider text-[10px]">
            {{ $t("digimon.allDigimon") }}
          </span>
        </template>
        <template v-else>
          {{ equipment.equipableDigimonNames.join(", ") }}
        </template>
      </span>
    </div>

    <p
      v-if="equipmentNote"
      class="pt-1 border-t border-[#0033aa]/50 text-gray-400 text-[10px] italic leading-tight"
    >
      "{{ equipmentNote }}"
    </p>
  </div>
</template>
