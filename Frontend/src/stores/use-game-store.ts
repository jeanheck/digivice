import { defineStore } from "pinia";
import { ref, computed } from "vue";
import type { State } from "@/models";
import type * as Events from "@/events/events.map";
import { HealthStatus } from "@/models/health-status";
import { storeLogger } from "@/events/logger";
import { PlayerConverter } from "@/events/converters/player.converter";
import { ImportantItemsConverter } from "@/events/converters/important-items.converter";
import { PartyConverter } from "@/events/converters/party.converter";
import { DigimonBattleConverter } from "@/events/converters/digimon-battle.converter";
import { CardBattleConverter } from "@/events/converters/card-battle.converter";
import { AuctionsConverter } from "@/events/converters/auctions.converter";
import { NpcsConverter } from "@/events/converters/npcs.converter";
import { JournalConverter } from "@/events/converters/journal.converter";
import { PlayerSyncer } from "./syncers/player.syncer";
import { ImportantItemsSyncer } from "./syncers/important-items.syncer";
import { JournalSyncer } from "./syncers/journal.syncer";
import { PartySyncer } from "./syncers/party.syncer";
import { DigimonBattleSyncer } from "./syncers/digimon-battle.syncer";
import { CardBattleSyncer } from "./syncers/card-battle.syncer";
import { AuctionsSyncer } from "./syncers/auctions.syncer";
import { NpcsSyncer } from "./syncers/npcs.syncer";

export const useGameStore = defineStore("game", () => {
  const isConnectedWithBackend = ref(false);
  const healthStatus = ref<HealthStatus>(HealthStatus.Loading);
  const backendProcessFailed = ref(false);
  const lastHubConnectionError = ref<string | null>(null);
  const lastErrorCode = ref<string | null>(null);
  const lastErrorDetail = ref<string | null>(null);
  const isConnected = computed(() => {
    return isConnectedWithBackend.value && healthStatus.value === HealthStatus.Healthy;
  });
  const currentState = ref<State | null>(null);

  function clearGameState(): void {
    currentState.value = null;
  }

  function setBackendProcessFailed(failed: boolean): void {
    backendProcessFailed.value = failed;
    if (failed) {
      isConnectedWithBackend.value = false;
      clearGameState();
    }
  }

  function syncHubConnectionStatus(event: {
    isConnected: boolean;
    errorMessage?: string;
    preserveGameState?: boolean;
  }): void {
    isConnectedWithBackend.value = event.isConnected;

    if (event.isConnected) {
      lastHubConnectionError.value = null;
      return;
    }

    if (!event.preserveGameState) {
      clearGameState();
    }

    if (event.errorMessage) {
      lastHubConnectionError.value = event.errorMessage;
    }
  }

  function syncHealth(healthDto: Events.HealthDTO): void {
    healthStatus.value = healthDto.status;

    if (healthDto.status === HealthStatus.Healthy) {
      lastErrorCode.value = null;
      lastErrorDetail.value = null;
      return;
    }

    if (healthDto.status === HealthStatus.Loading) {
      lastErrorCode.value = null;
      lastErrorDetail.value = null;
      return;
    }

    clearGameState();
    lastErrorCode.value = healthDto.errorCode;
    lastErrorDetail.value = healthDto.errorDetail;
  }

  function setInitialState(state: Events.StateDTO): void {
    currentState.value = {
      player: PlayerConverter.convert(state.player),
      importantItems: ImportantItemsConverter.convert(state.importantItems),
      party: PartyConverter.convert(state.party),
      digimonBattle: DigimonBattleConverter.convert(state.digimonBattle),
      cardBattle: CardBattleConverter.convert(state.cardBattle),
      auctions: AuctionsConverter.convert(state.auctions),
      npcs: NpcsConverter.convert(state.npcs),
      journal: JournalConverter.convert(state.journal),
    };
  }

  function getStateOrWarn(eventName: string): State | null {
    const state = currentState.value;
    if (!state) {
      storeLogger.warn(`${eventName} ignored: InitialState not received yet.`);
      return null;
    }

    return state;
  }

  function syncPlayer(newPlayerDto: Events.PlayerDTO): void {
    const state = getStateOrWarn("PlayerChanged");
    if (!state) {
      return;
    }

    PlayerSyncer.sync(state.player, newPlayerDto);
  }

  function syncImportantItems(newImportantItemsDto: Events.ImportantItemsDTO): void {
    const state = getStateOrWarn("ImportantItemsChanged");
    if (!state) {
      return;
    }

    ImportantItemsSyncer.sync(state.importantItems, newImportantItemsDto);
  }

  function syncJournal(newJournalDto: Events.JournalDTO): void {
    const state = getStateOrWarn("JournalChanged");
    if (!state) {
      return;
    }

    JournalSyncer.sync(state.journal, newJournalDto);
  }

  function syncParty(newPartyDto: Events.PartyDTO): void {
    const state = getStateOrWarn("PartyChanged");
    if (!state) {
      return;
    }

    PartySyncer.sync(state.party, newPartyDto);
  }

  function syncDigimonBattle(newDigimonBattleDto: Events.DigimonBattleDTO): void {
    const state = getStateOrWarn("DigimonBattleChanged");
    if (!state) {
      return;
    }

    DigimonBattleSyncer.sync(state.digimonBattle, newDigimonBattleDto);
  }

  function syncCardBattle(newCardBattleDto: Events.CardBattleDTO): void {
    const state = getStateOrWarn("CardBattleChanged");
    if (!state) {
      return;
    }

    CardBattleSyncer.sync(state.cardBattle, newCardBattleDto);
  }

  function syncAuctions(newAuctionsDto: Events.AuctionsDTO): void {
    const state = getStateOrWarn("AuctionsChanged");
    if (!state) {
      return;
    }

    AuctionsSyncer.sync(state.auctions, newAuctionsDto);
  }

  function syncNpcs(newNpcsDto: Events.NpcsDTO): void {
    const state = getStateOrWarn("NpcsChanged");
    if (!state) {
      return;
    }

    NpcsSyncer.sync(state.npcs, newNpcsDto);
  }

  return {
    isConnected,
    isConnectedWithBackend,
    healthStatus,
    backendProcessFailed,
    lastHubConnectionError,
    lastErrorCode,
    lastErrorDetail,
    setBackendProcessFailed,
    currentState,
    syncHubConnectionStatus,
    syncHealth,
    setInitialState,
    syncPlayer,
    syncImportantItems,
    syncParty,
    syncDigimonBattle,
    syncCardBattle,
    syncAuctions,
    syncNpcs,
    syncJournal,
  };
});
