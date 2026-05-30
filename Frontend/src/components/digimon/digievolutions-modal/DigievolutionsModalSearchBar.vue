<script setup lang="ts">
import { computed, ref } from "vue";

const props = defineProps<{
  allDigievolutionsNames: string[];
}>();

const emit = defineEmits<{
  (e: "select", name: string): void;
}>();

const searchQuery = ref("");
const showDropdown = ref(false);

const filteredDigievolutions = computed(() => {
  const query = searchQuery.value.toLowerCase();
  if (!query) {
    return [];
  }

  return props.allDigievolutionsNames.filter((digievolution) => {
    return digievolution.toLowerCase().includes(query);
  });
});

const handleSearchSelect = (name: string) => {
  emit("select", name);
  searchQuery.value = "";
  showDropdown.value = false;
};

const handleBlur = () => {
  showDropdown.value = false;
};
</script>

<template>
  <div class="relative w-lg min-w-0 flex-1 max-w-lg">
    <input
      v-model="searchQuery"
      type="text"
      :placeholder="$t('digievolution.searchDigimon')"
      class="w-full bg-[#001a33]/60 border border-[#0055ff]/50 rounded px-3 py-1 text-xs text-[#00aaff] placeholder-[#00aaff]/40 outline-none focus:border-[#00aaff] focus:bg-[#002244] font-cyber transition-all"
      @focus="showDropdown = true"
      @blur="handleBlur"
    />
    <div
      v-if="showDropdown && searchQuery && filteredDigievolutions.length > 0"
      class="absolute top-full left-0 right-0 mt-1 bg-[#001122] border border-[#0055ff]/50 rounded shadow-[0_4px_12px_rgba(0,119,255,0.2)] max-h-48 overflow-y-auto custom-scroll z-50 flex flex-col"
    >
      <div
        v-for="digievolution in filteredDigievolutions"
        :key="digievolution"
        class="px-3 py-1.5 text-xs text-[#00aaff] hover:bg-[#0033aa] hover:text-white cursor-pointer transition-colors font-cyber border-b last:border-b-0 border-[#0055ff]/20"
        @mousedown.prevent="handleSearchSelect(digievolution)"
      >
        {{ digievolution }}
      </div>
    </div>
  </div>
</template>
