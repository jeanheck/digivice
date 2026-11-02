<script setup lang="ts">
defineProps<{
  equipmentShopIds: string[];
}>();

const emit = defineEmits<{
  (e: "open-equipment-shop", equipmentShopId: string): void;
}>();

const handleSelect = (equipmentShopId: string): void => {
  emit("open-equipment-shop", equipmentShopId);
};
</script>

<template>
  <section class="shrink-0 w-1/2 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner">
    <h4 class="text-[10px] uppercase font-bold tracking-widest text-blue-500 mb-3">
      {{ $t("enemy.purchasableAt") }}
    </h4>

    <p v-if="equipmentShopIds.length === 0" class="text-xs text-gray-400 italic">
      {{ $t("enemy.purchasableAtNone") }}
    </p>
    <div v-else class="flex flex-wrap gap-2">
      <button
        v-for="equipmentShopId in equipmentShopIds"
        :key="equipmentShopId"
        type="button"
        class="flex items-center gap-2 px-2.5 py-2 rounded text-left transition-colors cursor-pointer bg-blue-900/30 hover:bg-blue-900/50 border border-blue-800/60"
        @click="handleSelect(equipmentShopId)"
      >
        <span class="min-w-0">
          <span class="block text-xs font-bold text-blue-200 tracking-wide">
            {{ $t(`equipmentShop.${equipmentShopId}.name`) }}
          </span>
        </span>
      </button>
    </div>
  </section>
</template>
