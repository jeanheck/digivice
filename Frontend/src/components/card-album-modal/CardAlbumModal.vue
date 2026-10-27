<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from "vue";
import { useI18n } from "vue-i18n";
import Modal from "@/components/modal/Modal.vue";
import Tooltip from "@/components/tooltip/Tooltip.vue";
import CardAlbumCard from "@/components/card-album-modal/CardAlbumCard.vue";
import WikiCardPanel from "@/components/wiki-modal/wiki-card-panel/WikiCardPanel.vue";
import WikiDropsPanel from "@/components/wiki-modal/wiki-drops-panel/WikiDropsPanel.vue";
import WikiCardShopPanel from "@/components/wiki-modal/wiki-card-shop-panel/WikiCardShopPanel.vue";
import WikiLocationsPanel from "@/components/wiki-modal/wiki-locations-panel/WikiLocationsPanel.vue";
import WikiEnemyPanel from "@/components/wiki-modal/wiki-enemy-panel/WikiEnemyPanel.vue";
import WikiNpcPanel from "@/components/wiki-modal/wiki-npc-panel/WikiNpcPanel.vue";
import SearchBar from "@/components/search/SearchBar.vue";
import { CardAlbumPresenter } from "@/presenters/card-album/card-album.presenter";
import { CardAlbumBoosterPresenter } from "@/presenters/card-album/card-album-booster.presenter";
import CardAlbumBoostersView from "@/components/card-album-modal/CardAlbumBoostersView.vue";
import { WikiModalPresenter } from "@/presenters/map/wiki-modal.presenter";
import { CardRepository } from "@/repositories/card.repository";
import { ImageCatalog } from "@/catalogs/image.catalog";
import { useGameState } from "@/composables/use-game-state";
import { useTooltipPosition } from "@/composables/use-tooltip-position";
import type { DropType } from "@/repositories/tables/raws/drop/drop-type";
import type { DropSourceKind } from "@/types/drop-source-kind.type";
import type { SearchItemKind } from "@/viewmodels/search/search-item.viewmodel";

const props = defineProps<{
  isOpen: boolean;
}>();

const emit = defineEmits<{
  (e: "close"): void;
}>();

const gameState = useGameState();
const { t } = useI18n();

// Mode State: "cards" (Cards album) vs "boosters" (Boosters completion overview)
type AlbumMode = "cards" | "boosters";
const albumMode = ref<AlbumMode>("cards");

const toggleAlbumMode = () => {
  albumMode.value = albumMode.value === "cards" ? "boosters" : "cards";
};

// Album Cards State
const currentPage = ref(1);

// Boosters Summary for Header Stats
const boostersSummary = computed(() => {
  return CardAlbumBoosterPresenter.getSummary(gameState.value.cardInventory);
});
const totalBoosters = computed(() => boostersSummary.value.totalBoosters);
const completedBoostersCount = computed(() => boostersSummary.value.completedCount);
const completedBoostersPercentage = computed(() => boostersSummary.value.percentage);


// Detail / Wiki Navigation State
type WikiView = "enemy" | "drops" | "cards" | "locations" | "npc" | "cardShops";

interface HistoryEntry {
  view: WikiView;
  selectedEnemyId: string | null;
  selectedDropId: string | null;
  selectedDropType: DropType | null;
  selectedCardId: string | null;
  selectedLocationId: string | null;
  selectedNpcId: string | null;
  selectedCardShopId: string | null;
  initialNpcBattleOptionId: string | null;
}

const view = ref<WikiView>("cards");
const selectedCardId = ref<string | null>(null);
const selectedDropId = ref<string | null>(null);
const selectedDropType = ref<DropType | null>(null);
const selectedEnemyId = ref<string | null>(null);
const selectedLocationId = ref<string | null>(null);
const selectedNpcId = ref<string | null>(null);
const selectedCardShopId = ref<string | null>(null);
const initialNpcBattleOptionId = ref<string | null>(null);
const navigationHistory = ref<HistoryEntry[]>([]);

const isDetailModalOpen = computed(() => {
  return (
    selectedCardId.value !== null ||
    selectedDropId.value !== null ||
    selectedCardShopId.value !== null ||
    selectedEnemyId.value !== null ||
    selectedLocationId.value !== null ||
    selectedNpcId.value !== null
  );
});

// Card sequential navigation (Next / Previous card)
const sortedCardIds = computed(() => {
  return CardRepository.getCardIds().sort((a, b) => Number(a) - Number(b));
});

const currentCardIndex = computed(() => {
  if (selectedCardId.value === null) {
    return -1;
  }
  return sortedCardIds.value.indexOf(selectedCardId.value);
});

const currentCardNumber = computed(() => {
  if (currentCardIndex.value === -1) {
    return 0;
  }
  return currentCardIndex.value + 1;
});

const hasPreviousCard = computed(() => {
  return currentCardIndex.value > 0;
});

const hasNextCard = computed(() => {
  return (
    currentCardIndex.value >= 0 &&
    currentCardIndex.value < sortedCardIds.value.length - 1
  );
});

const syncAlbumPageWithCurrentCard = () => {
  if (currentCardNumber.value > 0) {
    currentPage.value = Math.floor((currentCardNumber.value - 1) / 12) + 1;
  }
};

const goToPreviousCard = () => {
  if (!hasPreviousCard.value) {
    return;
  }
  const prevId = sortedCardIds.value[currentCardIndex.value - 1];
  if (prevId) {
    selectedCardId.value = prevId;
    syncAlbumPageWithCurrentCard();
  }
};

const goToNextCard = () => {
  if (!hasNextCard.value) {
    return;
  }
  const nextId = sortedCardIds.value[currentCardIndex.value + 1];
  if (nextId) {
    selectedCardId.value = nextId;
    syncAlbumPageWithCurrentCard();
  }
};

// Tooltip setup for enemy & NPC panels
const tooltipPlacement = "below" as const;
const { show: tooltipShow, x: tooltipX, y: tooltipY, showAt, move, hide } = useTooltipPosition(250);
const tooltipTitle = ref("");

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

// Album View Model
const pageViewModel = computed(() => {
  return CardAlbumPresenter.getPageViewModel(
    currentPage.value,
    gameState.value.cardInventory,
  );
});

const collectionPercentage = computed(() => {
  if (pageViewModel.value.totalCards === 0) {
    return "0.0";
  }
  return (
    (pageViewModel.value.totalUniqueOwned / pageViewModel.value.totalCards) *
    100
  ).toFixed(1);
});

// Search integration
const isLocationsView = computed(() => view.value === "locations");
const isDropsView = computed(() => view.value === "drops");
const isCardsView = computed(() => view.value === "cards");
const isNpcView = computed(() => view.value === "npc");
const isCardShopsView = computed(() => view.value === "cardShops");

const allSearchItems = computed(() => {
  return WikiModalPresenter.getAllSearchItems(
    (labelKey) => t(labelKey),
    (cardId) => t(`card.${cardId}.name`),
    (locationId) => t(`map.${locationId}`),
    (cardShopId) => t(`cardShop.${cardShopId}.name`),
    (tamerId) => t(`tamers.${tamerId}.name`),
    (duelIslandId) => t(`duelIsland.${duelIslandId}.name`),
    (npcId) => t(`npcs.${npcId}.name`),
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

const enemy = computed(() => {
  if (selectedEnemyId.value === null) {
    return null;
  }
  return WikiModalPresenter.getEnemyById(selectedEnemyId.value);
});

const enemyImageUrl = computed(() => {
  if (enemy.value === null) {
    return null;
  }
  return ImageCatalog.getEnemyImageUrl(enemy.value.name);
});

// Navigation handlers
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

function pushCurrentState(): void {
  navigationHistory.value.push({
    view: view.value,
    selectedEnemyId: selectedEnemyId.value,
    selectedDropId: selectedDropId.value,
    selectedDropType: selectedDropType.value,
    selectedCardId: selectedCardId.value,
    selectedLocationId: selectedLocationId.value,
    selectedNpcId: selectedNpcId.value,
    selectedCardShopId: selectedCardShopId.value,
    initialNpcBattleOptionId: initialNpcBattleOptionId.value,
  });
}

function navigateBack(): void {
  const previous = navigationHistory.value.pop();
  if (!previous) {
    return;
  }
  hide();
  clearSelection();
  view.value = previous.view;
  selectedEnemyId.value = previous.selectedEnemyId;
  selectedDropId.value = previous.selectedDropId;
  selectedDropType.value = previous.selectedDropType;
  selectedCardId.value = previous.selectedCardId;
  selectedLocationId.value = previous.selectedLocationId;
  selectedNpcId.value = previous.selectedNpcId;
  selectedCardShopId.value = previous.selectedCardShopId;
  initialNpcBattleOptionId.value = previous.initialNpcBattleOptionId;
}

function navigateTo(
  nextView: WikiView,
  id: string,
  options?: { battleOptionId?: string | null; dropType?: DropType },
): void {
  hide();
  pushCurrentState();
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
  const npcContext = WikiModalPresenter.resolveNpcBattleFromEnemyId(enemyId);
  if (npcContext !== null) {
    openNpcView(npcContext.npcId, npcContext.battleOptionId);
    return;
  }
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

const handleSelectCard = (cardId: string) => {
  hide();
  clearSelection();
  navigationHistory.value = [];
  view.value = "cards";
  selectedCardId.value = cardId;
};

const closeCardDetails = () => {
  hide();
  clearSelection();
  navigationHistory.value = [];
  view.value = "cards";
};

// Pagination for cards mode
const nextPage = () => {
  if (currentPage.value < CardAlbumPresenter.TOTAL_PAGES) {
    currentPage.value++;
  }
};

const previousPage = () => {
  if (currentPage.value > 1) {
    currentPage.value--;
  }
};

const firstPage = () => {
  currentPage.value = 1;
};

const lastPage = () => {
  currentPage.value = CardAlbumPresenter.TOTAL_PAGES;
};

const handleKeydown = (event: KeyboardEvent) => {
  if (!props.isOpen) {
    return;
  }

  if (isDetailModalOpen.value) {
    if (event.key === "Escape") {
      if (navigationHistory.value.length > 0) {
        navigateBack();
      } else {
        closeCardDetails();
      }
      return;
    }

    if (view.value === "cards" && selectedCardId.value !== null) {
      if (event.key === "ArrowLeft") {
        goToPreviousCard();
        return;
      }
      if (event.key === "ArrowRight") {
        goToNextCard();
        return;
      }
    }
    return;
  }

  if (albumMode.value === "cards") {
    if (event.key === "ArrowLeft") {
      previousPage();
    } else if (event.key === "ArrowRight") {
      nextPage();
    }
  }
};

onMounted(() => {
  window.addEventListener("keydown", handleKeydown);
});

onUnmounted(() => {
  window.removeEventListener("keydown", handleKeydown);
});

const handleClose = () => {
  if (isDetailModalOpen.value) {
    closeCardDetails();
    return;
  }
  emit("close");
};
</script>

<template>
  <Modal
    :is-open="isOpen"
    max-width="max-w-[1280px]"
    max-height="h-[710px] max-h-[710px]"
    panel-class="w-[1280px]"
    @close="handleClose"
  >
    <template #header>
      <div class="flex items-center justify-between w-full pr-4 gap-4">
        <!-- Title & Icon -->
        <div class="flex items-center gap-3 shrink-0">
          <span class="text-2xl leading-none">{{ albumMode === 'boosters' ? '🃏' : '🎴' }}</span>
          <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap text-lg">
            {{ albumMode === 'boosters' ? $t("cardAlbum.boosters") : $t("cardAlbum.title") }}
          </h2>
        </div>

        <!-- Collection Progress & Stats & Mode Toggle Button -->
        <div class="flex items-center gap-4 sm:gap-5 text-xs">
          <!-- Total copies in trunk badge -->
          <div class="hidden sm:flex items-center gap-2 bg-[#001433]/70 border border-blue-800/50 px-3 py-1 rounded-md text-blue-200 shadow-inner">
            <span class="text-gray-400 font-medium">{{ $t("cardAlbum.totalCopies") }}:</span>
            <span class="font-bold text-amber-400 font-mono">{{ pageViewModel.totalCopies }}</span>
          </div>

          <!-- Mode Toggle Button (Pacotes <-> Cartas) -->
          <button
            type="button"
            class="flex items-center gap-2 px-3 py-1 rounded-md border text-xs font-bold transition-all cursor-pointer shadow-sm select-none"
            :class="
              albumMode === 'boosters'
                ? 'bg-cyan-950/90 border-cyan-400 text-cyan-300 hover:bg-cyan-900/90 hover:text-white shadow-[0_0_12px_rgba(0,200,255,0.4)]'
                : 'bg-blue-950/90 border-blue-700 text-blue-300 hover:bg-blue-900 hover:text-white hover:border-cyan-400'
            "
            @click="toggleAlbumMode"
          >
            <span class="text-sm leading-none">{{ albumMode === 'boosters' ? '🎴' : '🃏' }}</span>
            <span>{{ albumMode === 'boosters' ? $t("cardAlbum.cards") : $t("cardAlbum.boosters") }}</span>
          </button>

          <!-- Overall Progress: Cards or Boosters completion -->
          <div class="flex items-center gap-3 bg-[#001433]/70 border border-blue-800/50 px-3 py-1 rounded-md shadow-inner">
            <span class="text-gray-400 font-medium">
              {{ albumMode === 'boosters' ? $t("cardAlbum.completedBoosters") : $t("cardAlbum.collection") }}:
            </span>
            <span class="font-bold text-emerald-400 font-mono">
              <template v-if="albumMode === 'boosters'">
                {{ completedBoostersCount }} / {{ totalBoosters }}
              </template>
              <template v-else>
                {{ pageViewModel.totalUniqueOwned }} / {{ pageViewModel.totalCards }}
              </template>
            </span>
            <span class="text-[10px] text-cyan-300 font-mono">
              ({{ albumMode === 'boosters' ? completedBoostersPercentage : collectionPercentage }}%)
            </span>

            <!-- Glowing Progress Bar -->
            <div class="w-24 sm:w-32 h-2.5 bg-blue-950 border border-blue-800 rounded-full overflow-hidden shrink-0 shadow-inner">
              <div
                class="h-full bg-linear-to-r from-cyan-500 via-teal-400 to-emerald-400 transition-all duration-300 rounded-full"
                :style="{ width: `${albumMode === 'boosters' ? completedBoostersPercentage : collectionPercentage}%` }"
              />
            </div>
          </div>
        </div>
      </div>
    </template>

    <!-- Main Content Area: Cards Album Mode or Boosters Mode -->
    <div class="relative flex flex-1 flex-col min-h-0 h-full p-4 overflow-hidden select-none">
      <!-- 1. CARDS MODE -->
      <template v-if="albumMode === 'cards'">
        <!-- 6x2 Cards Grid (12 Cards Per Page) -->
        <div class="flex-1 min-h-0 bg-[#000a18]/60 border border-blue-900/40 rounded-lg p-3 shadow-inner overflow-hidden">
          <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-6 gap-3 h-full">
            <CardAlbumCard
              v-for="(card, index) in pageViewModel.cards"
              :key="card ? card.cardId : `empty-slot-${index}`"
              :card="card"
              :slot-index="(currentPage - 1) * 12 + index + 1"
              @select="handleSelectCard"
            />
          </div>
        </div>

        <!-- Pagination Navigation Bar -->
        <div class="flex items-center justify-between mt-3 pt-2 border-t border-blue-900/50 shrink-0">
          <!-- Quick Jump Buttons (Previous / First) -->
          <div class="flex items-center gap-1.5">
            <button
              type="button"
              class="px-2.5 py-1 text-xs font-bold rounded bg-blue-950/80 border border-blue-800 text-blue-300 hover:bg-blue-900 hover:text-white disabled:opacity-30 disabled:pointer-events-none transition-colors cursor-pointer"
              :disabled="currentPage === 1"
              :title="$t('cardAlbum.firstPage')"
              @click="firstPage"
            >
              «
            </button>
            <button
              type="button"
              class="px-3 py-1 text-xs font-bold rounded bg-blue-950/80 border border-blue-800 text-blue-300 hover:bg-blue-900 hover:text-white disabled:opacity-30 disabled:pointer-events-none transition-colors cursor-pointer"
              :disabled="currentPage === 1"
              :title="$t('cardAlbum.previousPage')"
              @click="previousPage"
            >
              ‹ {{ $t("cardAlbum.previousPage") }}
            </button>
          </div>

          <!-- Current Page Indicator & Quick Page Select Dropdown -->
          <div class="flex items-center gap-2">
            <span class="text-xs text-gray-300 font-medium">
              {{ $t("cardAlbum.page") }}
            </span>

            <select
              v-model.number="currentPage"
              class="bg-[#000f2b] border border-blue-700 text-cyan-300 font-bold font-mono text-xs rounded px-2.5 py-1 outline-none cursor-pointer hover:border-cyan-400 focus:border-cyan-400 transition-colors"
            >
              <option
                v-for="p in CardAlbumPresenter.TOTAL_PAGES"
                :key="p"
                :value="p"
              >
                {{ p }} (Cards #{{ String((p - 1) * 12 + 1).padStart(3, "0") }} - #{{ String(Math.min(p * 12, 314)).padStart(3, "0") }})
              </option>
            </select>

            <span class="text-xs text-gray-300 font-medium">
              {{ $t("cardAlbum.of") }} {{ CardAlbumPresenter.TOTAL_PAGES }}
            </span>
          </div>

          <!-- Next & Last Page Buttons -->
          <div class="flex items-center gap-1.5">
            <button
              type="button"
              class="px-3 py-1 text-xs font-bold rounded bg-blue-950/80 border border-blue-800 text-blue-300 hover:bg-blue-900 hover:text-white disabled:opacity-30 disabled:pointer-events-none transition-colors cursor-pointer"
              :disabled="currentPage === CardAlbumPresenter.TOTAL_PAGES"
              :title="$t('cardAlbum.nextPage')"
              @click="nextPage"
            >
              {{ $t("cardAlbum.nextPage") }} ›
            </button>
            <button
              type="button"
              class="px-2.5 py-1 text-xs font-bold rounded bg-blue-950/80 border border-blue-800 text-blue-300 hover:bg-blue-900 hover:text-white disabled:opacity-30 disabled:pointer-events-none transition-colors cursor-pointer"
              :disabled="currentPage === CardAlbumPresenter.TOTAL_PAGES"
              :title="$t('cardAlbum.lastPage')"
              @click="lastPage"
            >
              »
            </button>
          </div>
        </div>
      </template>

      <!-- 2. BOOSTERS / CARDS OF BOOSTERS MODE -->
      <CardAlbumBoostersView
        v-else
        @open-booster="(boosterId) => openDropsView({ dropId: String(boosterId), dropType: 'booster' })"
        @switch-to-cards="albumMode = 'cards'"
      />
    </div>

    <!-- Wiki Detail Modal Overlay (Full Wiki Navigation from Cards or Boosters) -->
    <Teleport to="body">
      <Transition name="modal-fade">
        <div
          v-if="isDetailModalOpen"
          class="fixed inset-0 z-150 flex items-center justify-center p-4 bg-black/75 backdrop-blur-xs"
          @click.self="closeCardDetails"
        >
          <div
            class="relative w-full max-w-[1300px] h-[650px] max-h-[650px] bg-[#001122] border-2 border-[#0055ff] shadow-[0_0_20px_rgba(0,119,255,0.4)] rounded-lg flex flex-col overflow-hidden animate-modal-slide-up"
          >
            <!-- Hex Pattern Overlay -->
            <div class="absolute inset-0 opacity-[0.03] pointer-events-none" />

            <!-- Header matching Wiki Modal with SearchBar, Card Nav, and Back Button -->
            <div class="relative z-10 flex items-center justify-between p-4 border-b border-blue-900/60 bg-[#001433]/80 shrink-0">
              <div class="flex items-center gap-4 flex-1 min-w-0 pr-4">
                <!-- Back Button when navigating inside Wiki -->
                <button
                  v-if="navigationHistory.length > 0"
                  type="button"
                  class="px-2.5 py-1 rounded border border-blue-700 bg-blue-950 text-cyan-300 hover:bg-blue-900 hover:border-cyan-400 text-xs font-bold transition-colors cursor-pointer flex items-center gap-1 shrink-0"
                  title="Voltar"
                  @click="navigateBack"
                >
                  ‹
                </button>

                <div class="flex items-center gap-2 shrink-0 min-w-0">
                  <h2 class="text-white font-bold tracking-widest drop-shadow whitespace-nowrap">
                    {{ t("enemy.wiki") }}
                  </h2>
                </div>

                <div class="flex-1 max-w-md min-w-0">
                  <SearchBar
                    :items="allSearchItems"
                    :selected-id="selectedSearchId"
                    :selected-kind="selectedSearchKind"
                    :placeholder="t('enemy.searchPlaceholder')"
                    :no-results-label="t('enemy.searchNoResults')"
                    @select="handleSearchSelect"
                  />
                </div>
              </div>

              <!-- Right Section: Previous / Next Card Controls + Close button -->
              <div class="flex items-center shrink-0">
                <!-- Previous / Next Card Controls (only visible on cards view) -->
                <div
                  v-if="view === 'cards' && selectedCardId !== null"
                  class="flex items-center gap-1.5 shrink-0 mr-3"
                >
                  <button
                    type="button"
                    class="flex items-center justify-center w-7 h-7 rounded border border-blue-700 bg-blue-950 text-cyan-300 hover:bg-blue-900 hover:text-white hover:border-cyan-400 disabled:opacity-30 disabled:pointer-events-none transition-colors cursor-pointer text-sm font-bold shadow-xs"
                    :disabled="!hasPreviousCard"
                    :title="t('cardAlbum.previousCard')"
                    @click="goToPreviousCard"
                  >
                    ‹
                  </button>

                  <div class="px-2.5 py-0.5 rounded bg-[#000a1a] border border-blue-900/60 text-cyan-400 font-mono font-bold text-xs shadow-inner whitespace-nowrap">
                    #{{ String(currentCardNumber).padStart(3, "0") }} / {{ sortedCardIds.length }}
                  </div>

                  <button
                    type="button"
                    class="flex items-center justify-center w-7 h-7 rounded border border-blue-700 bg-blue-950 text-cyan-300 hover:bg-blue-900 hover:text-white hover:border-cyan-400 disabled:opacity-30 disabled:pointer-events-none transition-colors cursor-pointer text-sm font-bold shadow-xs"
                    :disabled="!hasNextCard"
                    :title="t('cardAlbum.nextCard')"
                    @click="goToNextCard"
                  >
                    ›
                  </button>
                </div>

                <!-- Close button -->
                <button
                  type="button"
                  class="w-7 h-7 flex items-center justify-center rounded border border-blue-800/80 bg-blue-950/80 text-gray-400 hover:text-white hover:border-cyan-400 hover:bg-blue-900 transition-colors cursor-pointer text-xs font-bold shrink-0"
                  @click="closeCardDetails"
                >
                  ✕
                </button>
              </div>
            </div>

            <!-- Body with Wiki Panels and Side Nav Arrows -->
            <div class="relative z-10 flex-1 min-h-0 overflow-hidden flex items-center">
              <!-- Left Floating Arrow for Previous Card -->
              <button
                v-if="view === 'cards' && selectedCardId !== null && hasPreviousCard"
                type="button"
                class="absolute left-1 z-20 w-8 h-12 flex items-center justify-center rounded-r bg-[#001433]/85 hover:bg-blue-900/95 border-y border-r border-cyan-500/60 text-cyan-300 hover:text-white hover:scale-105 transition-all cursor-pointer shadow-lg backdrop-blur-xs text-base font-bold select-none"
                :title="t('cardAlbum.previousCard')"
                @click="goToPreviousCard"
              >
                ‹
              </button>

              <div class="flex-1 h-full min-h-0 overflow-hidden">
                <WikiCardPanel
                  v-if="view === 'cards' && selectedCardId !== null"
                  :card-id="selectedCardId"
                  @open-drop="openDropsView"
                  @open-card-shop="openCardShopsView"
                />
                <WikiDropsPanel
                  v-else-if="view === 'drops' && selectedDropId !== null && selectedDropType !== null"
                  :drop-id="selectedDropId"
                  :drop-type="selectedDropType"
                  @open-source="openDropSource"
                  @open-card="openCardFromBooster"
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
                <WikiEnemyPanel
                  v-else-if="view === 'enemy' && enemy !== null"
                  :enemy="enemy"
                  :enemy-image-url="enemyImageUrl"
                  @open-drops="openDropsView"
                  @open-locations="openLocationsView"
                  @show-stat-key-tooltip="showEnemyStatKeyTooltip"
                  @show-condition-tooltip="showEnemyConditionTooltip"
                  @move-stat-tooltip="moveEnemyStatTooltip"
                  @hide-stat-tooltip="hideEnemyStatTooltip"
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
              </div>

              <!-- Right Floating Arrow for Next Card -->
              <button
                v-if="view === 'cards' && selectedCardId !== null && hasNextCard"
                type="button"
                class="absolute right-1 z-20 w-8 h-12 flex items-center justify-center rounded-l bg-[#001433]/85 hover:bg-blue-900/95 border-y border-l border-cyan-500/60 text-cyan-300 hover:text-white hover:scale-105 transition-all cursor-pointer shadow-lg backdrop-blur-xs text-base font-bold select-none"
                :title="t('cardAlbum.nextCard')"
                @click="goToNextCard"
              >
                ›
              </button>
            </div>

            <!-- Footer bar accent -->
            <div class="h-1 bg-linear-to-r from-blue-900 via-cyan-500 to-blue-900 shrink-0" />
          </div>
        </div>
      </Transition>
    </Teleport>
  </Modal>

  <Tooltip
    :show="tooltipShow"
    :x="tooltipX"
    :y="tooltipY"
    :title="tooltipTitle"
    :max-width="900"
    placement="below"
  />
</template>
