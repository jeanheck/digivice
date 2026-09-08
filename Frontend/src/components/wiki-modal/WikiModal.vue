<script setup lang="ts">
import { computed, ref, watch } from "vue";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import WikiEnemyPanel from "@/components/wiki-modal/wiki-enemy-panel/WikiEnemyPanel.vue";
import WikiDropsPanel from "@/components/wiki-modal/wiki-drops-panel/WikiDropsPanel.vue";
import WikiCardPanel from "@/components/wiki-modal/wiki-card-panel/WikiCardPanel.vue";
import WikiLocationsPanel from "@/components/wiki-modal/wiki-locations-panel/WikiLocationsPanel.vue";
import WikiNpcPanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcPanel.vue";
import WikiCardShopPanel from "@/components/wiki-modal/wiki-card-shop-panel/WikiCardShopPanel.vue";
import SearchBar from "@/components/search/SearchBar.vue";
import { useI18n } from "vue-i18n";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import { ImageCatalog } from "@/catalogs/image.catalog.ts";
import { WikiModalPresenter } from "@/presenters/map/wiki-modal.presenter";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { DropSourceKind } from "@/viewmodels/drop/drop-source.viewmodel";
import type { SearchItemKind } from "@/viewmodels/search/search-item.viewmodel";

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

type WikiView = "enemy" | "drops" | "cards" | "locations" | "npc" | "cardShops";

const selectedEnemyId = ref<string | null>(null);
const selectedDropId = ref<string | null>(null);
const selectedDropType = ref<DropType | null>(null);
const selectedCardId = ref<string | null>(null);
const selectedLocationId = ref<string | null>(null);
const selectedNpcId = ref<string | null>(null);
const initialNpcBattleOptionId = ref<string | null>(null);
const selectedCardShopId = ref<string | null>(null);
const view = ref<WikiView>("enemy");

const isModalOpen = computed(() => {
  return (
    props.isOpen &&
    (selectedEnemyId.value !== null ||
      selectedDropId.value !== null ||
      selectedCardId.value !== null ||
      selectedLocationId.value !== null ||
      selectedNpcId.value !== null ||
      selectedCardShopId.value !== null)
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

const isCardShopsView = computed(() => {
  return view.value === "cardShops";
});

const handleClose = () => {
  emit("close");
};

const allSearchItems = computed(() => {
  return WikiModalPresenter.getAllSearchItems(
    (labelKey) => {
      return t(labelKey);
    },
    (cardId) => {
      return t(`cards.${cardId}.name`);
    },
    (locationId) => {
      return t(`location.${locationId}`);
    },
    (cardShopId) => {
      return t(`cardShops.${cardShopId}.name`);
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

  if (isCardShopsView.value && selectedCardShopId.value !== null) {
    return selectedCardShopId.value;
  }

  return selectedEnemyId.value ?? undefined;
});

const selectedSearchKind = computed((): SearchItemKind | undefined => {
  if (isDropsView.value && selectedDropType.value !== null) {
    return selectedDropType.value;
  }

  if (isCardsView.value) {
    return "card";
  }

  if (isLocationsView.value) {
    return "location";
  }

  if (isCardShopsView.value) {
    return "cardShop";
  }

  if (isNpcView.value) {
    return undefined;
  }

  if (selectedEnemyId.value !== null) {
    return "enemy";
  }

  return undefined;
});

const tooltipPlacement = "below" as const;
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = useTooltipPosition(250);
const tooltipTitle = ref("");

function clearSelection(): void {
  selectedEnemyId.value = null;
  selectedDropId.value = null;
  selectedDropType.value = null;
  selectedCardId.value = null;
  selectedLocationId.value = null;
  selectedNpcId.value = null;
  selectedCardShopId.value = null;
  initialNpcBattleOptionId.value = null;
}

function navigateTo(
  nextView: WikiView,
  id: string,
  options?: { battleOptionId?: string | null; dropType?: DropType },
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
    selectedDropType.value = options?.dropType ?? null;
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

  if (nextView === "cardShops") {
    selectedCardShopId.value = id;
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

const handleSearchSelect = (payload: { id: string; kind?: SearchItemKind }) => {
  const { id, kind } = payload;

  if (kind === "enemy") {
    const npcContext = WikiModalPresenter.resolveNpcBattleFromEnemyId(id);
    if (npcContext !== null) {
      openNpcView(npcContext.npcId, npcContext.battleOptionId);
      return;
    }

    navigateTo("enemy", id);
    return;
  }

  if (WikiModalPresenter.isDropSearchKind(kind)) {
    navigateTo("drops", id, { dropType: kind });
    return;
  }

  if (kind === "card") {
    navigateTo("cards", id);
    return;
  }

  if (kind === "location") {
    navigateTo("locations", id);
    return;
  }

  if (kind === "cardShop") {
    navigateTo("cardShops", id);
    return;
  }

  if (WikiModalPresenter.isNpcSearchKind(kind)) {
    openNpcView(id);
  }
};

const openDropsView = (payload: { dropId: string; dropType: DropType }) => {
  navigateTo("drops", payload.dropId, { dropType: payload.dropType });
};

const openLocationsView = (locationId: string) => {
  navigateTo("locations", locationId);
};

const openCardShopsView = (cardShopId: string) => {
  navigateTo("cardShops", cardShopId);
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
          :selected-kind="selectedSearchKind"
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
      v-else-if="view === 'drops' && selectedDropId !== null && selectedDropType !== null"
      :drop-id="selectedDropId"
      :drop-type="selectedDropType"
      @open-source="openDropSource"
      @open-card="openCardFromBooster"
    />
    <WikiCardPanel
      v-else-if="view === 'cards' && selectedCardId !== null"
      :card-id="selectedCardId"
      @open-drop="openDropsView"
      @open-card-shop="openCardShopsView"
    />
    <WikiCardShopPanel
      v-else-if="view === 'cardShops' && selectedCardShopId !== null"
      :card-shop-id="selectedCardShopId"
      @open-card="openCardFromBooster"
      @open-location="openLocationsView"
    />
    <WikiLocationsPanel
      v-else-if="view === 'locations' && selectedLocationId !== null"
      :location-id="selectedLocationId"
      @open-enemy="openEnemyFromDropSource"
      @open-npc="openNpcView"
      @open-card-shop="openCardShopsView"
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
