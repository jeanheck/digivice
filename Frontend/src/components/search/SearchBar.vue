<script setup lang="ts">
import { computed, ref, watch } from "vue";
import type { SearchItemViewModel } from "@/viewmodels/search/search-item.viewmodel";

const props = defineProps<{
  items: SearchItemViewModel[];
  selectedId?: string;
  placeholder: string;
  noResultsLabel: string;
}>();

const emit = defineEmits<{
  (e: "select", id: string): void;
}>();

const searchInput = ref<HTMLInputElement | null>(null);
const searchQuery = ref("");
const showDropdown = ref(false);
const isFocused = ref(false);

const selectedItemName = computed(() => {
  if (props.selectedId === undefined) {
    return "";
  }

  return props.items.find((item) => {
    return item.id === props.selectedId;
  })?.name ?? "";
});

const filteredItems = computed(() => {
  const query = searchQuery.value.toLowerCase();
  if (!query) {
    return [];
  }

  return props.items.filter((item) => {
    return item.name.toLowerCase().includes(query);
  });
});

const inputValue = computed({
  get(): string {
    if (isFocused.value) {
      return searchQuery.value;
    }

    return selectedItemName.value;
  },
  set(value: string) {
    searchQuery.value = value;
  },
});

watch(() => props.selectedId, () => {
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

const handleSearchSelect = (id: string) => {
  emit("select", id);
  searchInput.value?.blur();
};
</script>

<template>
  <div class="relative min-w-0 flex-1 w-full">
    <input
      ref="searchInput"
      v-model="inputValue"
      type="text"
      :placeholder="placeholder"
      class="w-full bg-[#001a33]/60 border border-[#0055ff]/50 rounded px-3 py-1 text-xs text-[#00aaff] placeholder-[#00aaff]/40 outline-none focus:border-[#00aaff] focus:bg-[#002244] transition-all"
      @focus="handleFocus"
      @blur="handleBlur"
    />
    <div
      v-if="showDropdown && isFocused && searchQuery"
      class="absolute top-full left-0 right-0 mt-1 bg-[#001122] border border-[#0055ff]/50 rounded shadow-[0_4px_12px_rgba(0,119,255,0.2)] max-h-48 overflow-y-auto custom-scroll z-50 flex flex-col"
    >
      <template v-if="filteredItems.length > 0">
        <div
          v-for="item in filteredItems"
          :key="item.id"
          class="px-3 py-1.5 text-xs text-[#00aaff] hover:bg-[#0033aa] hover:text-white cursor-pointer transition-colors border-b last:border-b-0 border-[#0055ff]/20"
          @mousedown.prevent="handleSearchSelect(item.id)"
        >
          {{ item.name }}
        </div>
      </template>
      <p
        v-else
        class="px-3 py-2 text-xs text-[#00aaff]/50 italic"
      >
        {{ noResultsLabel }}
      </p>
    </div>
  </div>
</template>
