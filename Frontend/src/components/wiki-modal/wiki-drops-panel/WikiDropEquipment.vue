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
  <div class="flex flex-col min-h-0 overflow-y-auto custom-scroll text-xs text-center gap-4">
    <div
      v-if="equipmentType"
      class="flex flex-col gap-1"
    >
      <p class="bg-[#002266]/40 px-2 py-1 rounded-sm text-blue-500 uppercase font-bold">
        {{ $t("enemy.type") }}
      </p>
      <p class="text-gray-100 uppercase font-bold">
        {{ equipmentType }}
      </p>
    </div>

    <div
      v-if="equipment.attributes.length > 0"
      class="flex flex-col gap-1"
    >
      <p class="bg-[#002266]/40 px-2 py-1 rounded-sm text-blue-500 uppercase font-bold">
        {{ $t("enemy.attr") }}
      </p>
      <div class="flex flex-col gap-0.5 w-1/5 mx-auto">
        <div
          v-for="attribute in equipment.attributes"
          :key="attribute.attribute"
          class="flex justify-between text-xs items-center px-2 py-1 rounded-sm"
        >
          <span class="text-gray-100 uppercase">{{ t("stat." + attribute.attribute.toLowerCase()) }}</span>
          <span
            :class="attribute.type === '+' ? 'text-green-400' : 'text-red-400'"
            class="font-bold tracking-wider"
          >
            {{ attribute.type }}{{ attribute.value }}
          </span>
        </div>
      </div>
    </div>

    <div class="flex flex-col gap-1">
      <p class="bg-[#002266]/40 px-2 py-1 rounded-sm text-blue-500 uppercase font-bold">
        {{ $t("digimon.equipableBy") }}
      </p>
      <p class="text-gray-100 text-[11px] leading-tight">
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
      </p>
    </div>

    <p
      v-if="equipmentNote"
      class="mt-2 text-gray-400 text-[10px] italic leading-tight"
    >
      "{{ equipmentNote }}"
    </p>
  </div>
</template>
