<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import WikiEnemyPanel from "@/components/wiki-modal/wiki-profile-panel/WikiEnemyPanel.vue";
import WikiDropsPanel from "@/components/wiki-modal/wiki-drops-panel/WikiDropsPanel.vue";
import WikiCardsPanel from "@/components/wiki-modal/wiki-cards-panel/WikiCardsPanel.vue";
import WikiLocationsPanel from "@/components/wiki-modal/wiki-locations-panel/WikiLocationsPanel.vue";
import WikiNpcPanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcPanel.vue";
import WikiStorePanel from "@/components/wiki-modal/wiki-stores-panel/WikiStorePanel.vue";
import SearchBar from "@/components/search/SearchBar.vue";
import { useI18n } from "vue-i18n";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { WikiModalPresenter } from "@/presenters/map/wiki-modal.presenter";
import { WikiProfileDropsPresenter } from "@/presenters/map/wiki-modal/wiki-profile-drops.presenter";
import type { DropSourceKind } from "@/viewmodels/drop/drop-source.viewmodel";

const props = defineProps<{
  isOpen: boolean;
  enemyId: string | null;
  locationId?: string | null;
  npcId?: string | null;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const { t } = useI18n();

type WikiView = "enemy" | "drops" | "cards" | "locations" | "npc" | "stores";

const selectedEnemyId = ref<string | null>(null);
const selectedDropId = ref<string | null>(null);
const selectedCardId = ref<string | null>(null);
const selectedLocationId = ref<string | null>(null);
const selectedNpcId = ref<string | null>(null);
const initialNpcBattleOptionId = ref<string | null>(null);
const selectedStoreId = ref<string | null>(null);
const view = ref<WikiView>("enemy");

const isModalOpen = computed(() => {
  return (
    props.isOpen &&
    (selectedEnemyId.value !== null ||
      selectedDropId.value !== null ||
      selectedCardId.value !== null ||
      selectedLocationId.value !== null ||
      selectedNpcId.value !== null ||
      selectedStoreId.value !== null)
  );
});

const isLocationsView = computed(() => {
  return view.value === "locations";
});

const isDropsView = computed(() => {
  return view.value === "drops";
});

const isCardsView = computed(() => {
  return view.value === "cards";
});

const isNpcView = computed(() => {
  return view.value === "npc";
});

const isStoresView = computed(() => {
  return view.value === "stores";
});

const handleClose = () => {
  emit("close");
};

const allSearchItems = computed(() => {
  return WikiModalPresenter.getAllSearchItems(
    (dropKey) => {
      return t(WikiProfileDropsPresenter.getDropLabelKey(dropKey));
    },
    (cardId) => {
      return t(`cards.${cardId}.name`);
    },
    (locationId) => {
      return t(`location.${locationId}`);
    },
    (storeId) => {
      return t(`stores.${storeId}.name`);
    },
    (tamerId) => {
      return t(`tamers.${tamerId}.name`);
    },
    (duelIslandId) => {
      return t(`duelIsland.${duelIslandId}.name`);
    },
    (npcId) => {
      return t(`npcs.${npcId}.name`);
    },
  );
});

const selectedSearchId = computed(() => {
  if (isDropsView.value && selectedDropId.value !== null) {
    return selectedDropId.value;
  }

  if (isCardsView.value && selectedCardId.value !== null) {
    return selectedCardId.value;
  }

  if (isLocationsView.value && selectedLocationId.value !== null) {
    return selectedLocationId.value;
  }

  if (isNpcView.value && selectedNpcId.value !== null) {
    return selectedNpcId.value;
  }

  if (isStoresView.value && selectedStoreId.value !== null) {
    return selectedStoreId.value;
  }

  return selectedEnemyId.value ?? undefined;
});

const tooltipPlacement = "below" as const;
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = useTooltipPosition(250);
const tooltipTitle = ref("");

function clearSelection(): void {
  selectedEnemyId.value = null;
  selectedDropId.value = null;
  selectedCardId.value = null;
  selectedLocationId.value = null;
  selectedNpcId.value = null;
  selectedStoreId.value = null;
  initialNpcBattleOptionId.value = null;
}

function navigateTo(
  nextView: WikiView,
  id: string,
  options?: { battleOptionId?: string | null },
): void {
  hide();
  clearSelection();
  view.value = nextView;

  if (nextView === "enemy") {
    selectedEnemyId.value = id;
    return;
  }

  if (nextView === "drops") {
    selectedDropId.value = id;
    return;
  }

  if (nextView === "cards") {
    selectedCardId.value = id;
    return;
  }

  if (nextView === "locations") {
    selectedLocationId.value = id;
    return;
  }

  if (nextView === "stores") {
    selectedStoreId.value = id;
    return;
  }

  if (nextView === "npc") {
    selectedNpcId.value = id;
    initialNpcBattleOptionId.value = options?.battleOptionId ?? null;
  }
}

const openNpcView = (npcId: string, battleOptionId?: string | null) => {
  navigateTo("npc", npcId, { battleOptionId });
};

const handleSearchSelect = (id: string) => {
  const searchItem = allSearchItems.value.find((item) => {
    return item.id === id;
  });
  if (searchItem === undefined) {
    return;
  }

  if (searchItem.kind === "enemy") {
    const npcContext = WikiModalPresenter.resolveNpcBattleFromEnemyId(id);
    if (npcContext !== null) {
      openNpcView(npcContext.npcId, npcContext.battleOptionId);
      return;
    }

    navigateTo("enemy", id);
    return;
  }

  if (searchItem.kind === "drop") {
    navigateTo("drops", id);
    return;
  }

  if (searchItem.kind === "card") {
    navigateTo("cards", id);
    return;
  }

  if (searchItem.kind === "location") {
    navigateTo("locations", id);
    return;
  }

  if (searchItem.kind === "store") {
    navigateTo("stores", id);
    return;
  }

  if (WikiModalPresenter.isNpcSearchKind(searchItem.kind)) {
    openNpcView(id);
  }
};

const openDropsView = (dropId: string) => {
  navigateTo("drops", dropId);
};

const openLocationsView = (locationId: string) => {
  navigateTo("locations", locationId);
};

const openStoresView = (storeId: string) => {
  navigateTo("stores", storeId);
};

const openEnemyFromDropSource = (enemyId: string) => {
  navigateTo("enemy", enemyId);
};

const openDropSource = (payload: { kind: DropSourceKind; sourceId: string }) => {
  if (payload.kind === "enemy") {
    openEnemyFromDropSource(payload.sourceId);
    return;
  }

  openNpcView(payload.sourceId);
};

const openCardFromBooster = (cardId: string) => {
  navigateTo("cards", cardId);
};

const enemy = computed(() => {
  if (selectedEnemyId.value === null) {
    return null;
  }

  return WikiModalPresenter.getEnemyById(selectedEnemyId.value);
});

const showEnemyStatKeyTooltip = (event: MouseEvent, statKey: string) => {
  tooltipTitle.value = t(`stat.${statKey}`);
  showAt(event, { maxWidth: 250, placement: tooltipPlacement });
};

const showEnemyConditionTooltip = (event: MouseEvent, tooltipKey: string) => {
  tooltipTitle.value = t(tooltipKey);
  showAt(event, { maxWidth: 250, placement: tooltipPlacement });
};

const hideEnemyStatTooltip = () => {
  hide();
};

const moveEnemyStatTooltip = (event: MouseEvent) => {
  move(event, tooltipPlacement);
};

watch(
  () => props.isOpen,
  (open) => {
    if (open) {
      if (props.npcId !== null && props.npcId !== undefined) {
        openNpcView(props.npcId);
        return;
      }

      if (props.locationId !== null && props.locationId !== undefined) {
        navigateTo("locations", props.locationId);
        return;
      }

      if (props.enemyId !== null) {
        navigateTo("enemy", props.enemyId);
      }

      return;
    }

    hide();
    clearSelection();
    view.value = "enemy";
  },
);

const enemyImageUrl = computed(() => {
  if (enemy.value === null) {
    return null;
  }

  return ImageCatalog.getEnemyImageUrl(enemy.value.name);
});
</script>

<template>
  <Modal
    :is-open="isModalOpen"
    max-width="max-w-[1300px]"
    max-height="h-[650px] max-h-[650px]"
    panel-class="w-[1300px]"
    @close="handleClose"
  >
    <template #header>
      <div class="flex items-center gap-6 flex-1 min-w-0">
        <div class="flex items-center gap-2 shrink-0 min-w-0">
          <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap">
            {{ t(`enemy.wiki`) }}
          </h2>
        </div>

        <SearchBar
          :items="allSearchItems"
          :selected-id="selectedSearchId"
          :placeholder="t('enemy.searchPlaceholder')"
          :no-results-label="t('enemy.searchNoResults')"
          @select="handleSearchSelect"
        />
      </div>
    </template>

    <WikiEnemyPanel
      v-if="view === 'enemy' && enemy !== null"
      :enemy="enemy"
      :enemy-image-url="enemyImageUrl"
      @open-drops="openDropsView"
      @open-locations="openLocationsView"
      @show-stat-key-tooltip="showEnemyStatKeyTooltip"
      @show-condition-tooltip="showEnemyConditionTooltip"
      @move-stat-tooltip="moveEnemyStatTooltip"
      @hide-stat-tooltip="hideEnemyStatTooltip"
    />
    <WikiDropsPanel
      v-else-if="view === 'drops' && selectedDropId !== null"
      :drop-id="selectedDropId"
      @open-source="openDropSource"
      @open-card="openCardFromBooster"
    />
    <WikiCardsPanel
      v-else-if="view === 'cards' && selectedCardId !== null"
      :card-id="selectedCardId"
      @open-drop="openDropsView"
      @open-store="openStoresView"
    />
    <WikiStorePanel
      v-else-if="view === 'stores' && selectedStoreId !== null"
      :store-id="selectedStoreId"
      @open-card="openCardFromBooster"
      @open-location="openLocationsView"
    />
    <WikiLocationsPanel
      v-else-if="view === 'locations' && selectedLocationId !== null"
      :location-id="selectedLocationId"
      @open-enemy="openEnemyFromDropSource"
      @open-npc="openNpcView"
      @open-store="openStoresView"
    />
    <WikiNpcPanel
      v-else-if="view === 'npc' && selectedNpcId !== null"
      :npc-id="selectedNpcId"
      :initial-battle-option-id="initialNpcBattleOptionId"
      @open-locations="openLocationsView"
      @open-drops="openDropsView"
      @open-card="openCardFromBooster"
      @show-stat-key-tooltip="showEnemyStatKeyTooltip"
      @show-condition-tooltip="showEnemyConditionTooltip"
      @move-stat-tooltip="moveEnemyStatTooltip"
      @hide-stat-tooltip="hideEnemyStatTooltip"
    />
  </Modal>

  <Tooltip
    :show="tooltipShow"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :max-width="600"
    placement="below"
  />
</template>
