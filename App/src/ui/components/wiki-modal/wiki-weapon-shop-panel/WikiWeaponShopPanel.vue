<script setup lang="ts">
import { computed } from "vue";
import WikiLocatedIn from "@/ui/components/wiki-modal/WikiLocatedIn.vue";
import WikiWeaponShopInventory from "@/ui/components/wiki-modal/wiki-weapon-shop-panel/WikiWeaponShopInventory.vue";
import { WikiWeaponShopPanelPresenter } from "@/presentation/presenters/map/wiki-modal/wiki-weapon-shop-panel.presenter";

const props = defineProps<{
  weaponShopId: string;
}>();

const emit = defineEmits<{
  (e: "open-equipment", equipmentId: string): void;
  (e: "open-location", locationId: string): void;
}>();

const weaponShopViewModel = computed(() => {
  return WikiWeaponShopPanelPresenter.getViewModel(props.weaponShopId);
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
      <WikiWeaponShopInventory
        :items="weaponShopViewModel.items"
        @open-equipment="handleOpenEquipment"
      />
    </section>

    <WikiLocatedIn
      v-if="weaponShopViewModel.locationId !== null"
      :location-id="weaponShopViewModel.locationId"
      @open-location="handleOpenLocation"
    />
  </div>
</template>
