import { defineStore } from "pinia";
import { ref, computed } from "vue";
import type { State } from "@/models";
import type * as Events from "@/events/events.map";
import { HealthStatus } from "@/models/health-status";
import { HealthConverter } from "@/events/converters/health.converter";
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

  function syncHealth(data: Events.HealthDTO): void {
    const event = HealthConverter.convert(data);
    healthStatus.value = event.status;

    if (event.status === HealthStatus.Healthy) {
      lastErrorCode.value = null;
      lastErrorDetail.value = null;
      return;
    }

    if (event.status === HealthStatus.Loading) {
      lastErrorCode.value = null;
      lastErrorDetail.value = null;
      return;
    }

    clearGameState();
    lastErrorCode.value = event.errorCode ?? null;
    lastErrorDetail.value = event.errorDetail ?? null;
  }

  function setInitialState(state: Events.StateDTO): void {
    currentState.value = {
      player: state.player ? PlayerConverter.convert(state.player) : null,
      importantItems: state.importantItems ? ImportantItemsConverter.convert(state.importantItems) : null,
      party: state.party ? PartyConverter.convert(state.party) : null,
      digimonBattle: state.digimonBattle ? DigimonBattleConverter.convert(state.digimonBattle) : null,
      cardBattle: state.cardBattle ? CardBattleConverter.convert(state.cardBattle) : null,
      auctions: state.auctions ? AuctionsConverter.convert(state.auctions) : null,
      npcs: state.npcs ? NpcsConverter.convert(state.npcs) : null,
      journal: state.journal ? JournalConverter.convert(state.journal) : null,
    };
  }

  function syncPlayer(newPlayerDto: Events.PlayerDTO): void {
    const previousPlayer = currentState.value?.player;
    if (!previousPlayer) {
      return;
    }

    PlayerSyncer.sync(previousPlayer, newPlayerDto);
  }

  function syncImportantItems(newImportantItemsDto: Events.ImportantItemsDTO): void {
    const previousImportantItems = currentState.value?.importantItems;
    if (!previousImportantItems) {
      return;
    }

    ImportantItemsSyncer.sync(previousImportantItems, newImportantItemsDto);
  }

  function syncJournal(newJournalDto: Events.JournalDTO): void {
    const previousJournal = currentState.value?.journal;
    if (!previousJournal) {
      return;
    }

    JournalSyncer.sync(previousJournal, newJournalDto);
  }

  function syncParty(newPartyDto: Events.PartyDTO): void {
    const previousParty = currentState.value?.party;
    if (!previousParty) {
      return;
    }

    PartySyncer.sync(previousParty, newPartyDto);
  }

  function syncDigimonBattle(newDigimonBattleDto: Events.DigimonBattleDTO): void {
    const previousDigimonBattle = currentState.value?.digimonBattle;
    if (!previousDigimonBattle) {
      return;
    }

    DigimonBattleSyncer.sync(previousDigimonBattle, newDigimonBattleDto);
  }

  function syncCardBattle(newCardBattleDto: Events.CardBattleDTO): void {
    const previousCardBattle = currentState.value?.cardBattle;
    if (!previousCardBattle) {
      return;
    }

    CardBattleSyncer.sync(previousCardBattle, newCardBattleDto);
  }

  function syncAuctions(newAuctionsDto: Events.AuctionsDTO): void {
    const previousAuctions = currentState.value?.auctions;
    if (!previousAuctions) {
      return;
    }

    AuctionsSyncer.sync(previousAuctions, newAuctionsDto);
  }

  function syncNpcs(newNpcsDto: Events.NpcsDTO): void {
    const previousNpcs = currentState.value?.npcs;
    if (!previousNpcs) {
      return;
    }

    NpcsSyncer.sync(previousNpcs, newNpcsDto);
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
