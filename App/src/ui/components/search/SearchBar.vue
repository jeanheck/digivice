<script setup lang="ts">
import { computed, ref, watch } from "vue";
import { SearchItemSortHelper } from "@/presentation/presenters/helper/search-item-sort.helper";
import type {
  SearchItemKind,
  SearchItemViewModel,
} from "@/presentation/viewmodels/search/search-item.viewmodel";

const props = defineProps<{
  items: SearchItemViewModel[];
  selectedId?: string;
  selectedKind?: SearchItemKind;
  placeholder: string;
  noResultsLabel: string;
}>();

const emit = defineEmits<{
  (e: "select", payload: { id: string; kind?: SearchItemKind }): void;
}>();

const searchInput = ref<HTMLInputElement | null>(null);
const searchQuery = ref("");
const showDropdown = ref(false);
const isFocused = ref(false);

const selectedItemName = computed(() => {
  if (props.selectedId === undefined) {
    return "";
  }

  return (
    props.items.find((item) => {
      if (item.id !== props.selectedId) {
        return false;
      }
      if (props.selectedKind === undefined) {
        return true;
      }
      return item.kind === props.selectedKind;
    })?.name ?? ""
  );
});

const filteredItems = computed(() => {
  const query = searchQuery.value.toLowerCase();
  if (!query) {
    return [];
  }

  return SearchItemSortHelper.sort(
    props.items.filter((item) => {
      return item.name.toLowerCase().includes(query);
    }),
  );
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

watch(
  () => [props.selectedId, props.selectedKind],
  () => {
    searchQuery.value = "";
    showDropdown.value = false;
    isFocused.value = false;
  },
);

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

const handleSearchSelect = (item: SearchItemViewModel) => {
  emit("select", { id: item.id, kind: item.kind });
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
      class="w-full bg-surface/60 border border-frame/60 rounded px-3 py-1 text-xs text-accent-bright placeholder-accent/55 outline-none focus:border-accent focus:bg-surface-deep transition-all"
      @focus="handleFocus"
      @blur="handleBlur"
    />
    <div
      v-if="showDropdown && isFocused && searchQuery"
      class="absolute top-full left-0 right-0 mt-1 bg-surface border border-frame/50 rounded shadow-[0_4px_12px_color-mix(in_srgb,var(--color-accent)_25%,transparent)] max-h-48 overflow-y-auto custom-scroll z-50 flex flex-col"
    >
      <template v-if="filteredItems.length > 0">
        <div
          v-for="item in filteredItems"
          :key="`${item.kind}-${item.id}`"
          class="px-3 py-1.5 text-xs text-accent-bright hover:bg-accent/25 hover:text-ink cursor-pointer transition-colors border-b last:border-b-0 border-frame/25 flex items-baseline gap-2"
          @mousedown.prevent="handleSearchSelect(item)"
        >
          <span class="leading-none">{{ item.name }}</span>
          <span
            v-if="item.kindLabelKey !== undefined || item.kind"
            class="text-[10px] leading-none text-accent"
          >
            {{
              item.kindLabelKey !== undefined
                ? item.kindLabelParams !== undefined
                  ? $t(item.kindLabelKey, item.kindLabelParams)
                  : $t(item.kindLabelKey)
                : $t(`enemy.searchKind.${item.kind}`)
            }}
          </span>
        </div>
      </template>
      <p
        v-else
        class="px-3 py-2 text-xs text-accent/70 italic"
      >
        {{ noResultsLabel }}
      </p>
    </div>
  </div>
</template>
