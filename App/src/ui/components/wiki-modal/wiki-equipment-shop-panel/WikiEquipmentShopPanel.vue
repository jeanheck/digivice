<script setup lang="ts">
import { computed } from "vue";
import { useI18n } from "vue-i18n";
import WikiLocatedIn from "@/ui/components/wiki-modal/WikiLocatedIn.vue";
import WikiEquipmentShopInventory from "@/ui/components/wiki-modal/equipment-shop/WikiEquipmentShopInventory.vue";
import { WikiEquipmentShopPanelPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-equipment-shop-panel.presenter";

const props = defineProps<{
  equipmentShopId: string;
}>();

const emit = defineEmits<{
  (e: "open-equipment", equipmentId: string): void;
  (e: "open-location", locationId: string): void;
}>();

const { t } = useI18n();

const equipmentShopViewModel = computed(() => {
  return WikiEquipmentShopPanelPresenter.getViewModel(props.equipmentShopId, (equipmentId) => {
    return t(`equipments.${equipmentId}.name`);
  });
});

const handleOpenEquipment = (equipmentId: string): void => {
  emit("open-equipment", equipmentId);
};

const handleOpenLocation = (locationId: string): void => {
  emit("open-location", locationId);
};
</script>

<template>
  <div class="p-4 flex flex-col gap-4 h-full min-h-0 overflow-hidden">
    <section
      class="flex-1 w-full min-h-0 bg-[#000a1a] border border-blue-900/50 rounded p-4 shadow-inner flex flex-col"
    >
      <WikiEquipmentShopInventory
        :items="equipmentShopViewModel.items"
        @open-equipment="handleOpenEquipment"
      />
    </section>

    <WikiLocatedIn
      v-if="equipmentShopViewModel.locationId !== null"
      :location-id="equipmentShopViewModel.locationId"
      @open-location="handleOpenLocation"
    />
  </div>
</template>
