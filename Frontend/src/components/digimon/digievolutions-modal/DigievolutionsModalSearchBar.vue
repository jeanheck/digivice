<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { DigievolutionsModalDigievolutionLinkViewModel } from "@/viewmodels/digievolution/digievolutions-modal-digievolution-link.viewmodel";

const props = defineProps<{
  allDigievolutions: DigievolutionsModalDigievolutionLinkViewModel[];
  selectedDigievolutionId?: number;
}>();

const emit = defineEmits<{
  (e: "select-digievolution-id", digievolutionId: number): void;
}>();

const searchInput = ref<HTMLInputElement | null>(null);
const searchQuery = ref("");
const showDropdown = ref(false);
const isFocused = ref(false);

const selectedDigievolutionName = computed(() => {
  if (props.selectedDigievolutionId === undefined) {
    return "";
  }

  return props.allDigievolutions.find((digievolution) => {
    return digievolution.id === props.selectedDigievolutionId;
  })?.name ?? "";
});

const filteredDigievolutions = computed(() => {
  const query = searchQuery.value.toLowerCase();
  if (!query) {
    return [];
  }

  return props.allDigievolutions.filter((digievolution) => {
    return digievolution.name.toLowerCase().includes(query);
  });
});

const inputValue = computed({
  get(): string {
    if (isFocused.value) {
      return searchQuery.value;
    }

    return selectedDigievolutionName.value;
  },
  set(value: string) {
    searchQuery.value = value;
  },
});

watch(() => props.selectedDigievolutionId, () => {
  searchQuery.value = "";
  showDropdown.value = false;
  isFocused.value = false;
});

const handleFocus = () => {
  isFocused.value = true;
  searchQuery.value = "";
  showDropdown.value = true;
};

const handleBlur = () => {
  isFocused.value = false;
  showDropdown.value = false;
  searchQuery.value = "";
};

const handleSearchSelect = (digievolutionId: number) => {
  emit("select-digievolution-id", digievolutionId);
  searchInput.value?.blur();
};
</script>

<template>
  <div class="relative w-lg min-w-0 flex-1 max-w-lg">
    <input
      ref="searchInput"
      v-model="inputValue"
      type="text"
      :placeholder="$t('digievolution.searchDigimon')"
      class="w-full bg-[#001a33]/60 border border-[#0055ff]/50 rounded px-3 py-1 text-xs text-[#00aaff] placeholder-[#00aaff]/40 outline-none focus:border-[#00aaff] focus:bg-[#002244] font-cyber transition-all"
      @focus="handleFocus"
      @blur="handleBlur"
    />
    <div
      v-if="showDropdown && isFocused && searchQuery"
      class="absolute top-full left-0 right-0 mt-1 bg-[#001122] border border-[#0055ff]/50 rounded shadow-[0_4px_12px_rgba(0,119,255,0.2)] max-h-48 overflow-y-auto custom-scroll z-50 flex flex-col"
    >
      <template v-if="filteredDigievolutions.length > 0">
        <div
          v-for="digievolution in filteredDigievolutions"
          :key="digievolution.id"
          class="px-3 py-1.5 text-xs text-[#00aaff] hover:bg-[#0033aa] hover:text-white cursor-pointer transition-colors font-cyber border-b last:border-b-0 border-[#0055ff]/20"
          @mousedown.prevent="handleSearchSelect(digievolution.id)"
        >
          {{ digievolution.name }}
        </div>
      </template>
      <p
        v-else
        class="px-3 py-2 text-xs text-[#00aaff]/50 font-cyber italic"
      >
        {{ $t("digievolution.searchNoResults") }}
      </p>
    </div>
  </div>
</template>
