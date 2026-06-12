import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { State } from '../models';
import type { EmulatorConnectionStatus } from "../models/emulator-connection-status";
import type * as Events from '../events/events.map';
import { PlayerConverter } from '../events/converters/player.converter';
import { PartyConverter } from '../events/converters/party.converter';
import { JournalConverter } from '../events/converters/journal.converter';
import { PlayerSyncer } from './syncers/player.syncer';
import { JournalSyncer } from './syncers/journal.syncer';
import { PartySyncer } from './syncers/party.syncer';

export const useGameStore = defineStore('game', () => {
    const isConnectedWithBackend = ref(false);
    const isConnectedWithEmulator = ref(false);
    const backendProcessFailed = ref(false);
    const lastHubConnectionError = ref<string | null>(null);
    const lastEmulatorConnectionErrorCode = ref<string | null>(null);
    const lastEmulatorConnectionErrorDetail = ref<string | null>(null);
    const isConnected = computed(() => {
        return isConnectedWithBackend.value && isConnectedWithEmulator.value;
    });
    const currentState = ref<State | null>(null);

    function setBackendProcessFailed(failed: boolean): void {
        backendProcessFailed.value = failed;
        if (failed) {
            isConnectedWithBackend.value = false;
            currentState.value = null;
        }
    }

    function syncHubConnectionStatus(event: { isConnected: boolean; errorMessage?: string }): void {
        isConnectedWithBackend.value = event.isConnected;

        if (event.isConnected) {
            lastHubConnectionError.value = null;
            return;
        }

        if (event.errorMessage) {
            lastHubConnectionError.value = event.errorMessage;
        }
    }

    function syncEmulatorConnectionStatus(event: EmulatorConnectionStatus): void {
        isConnectedWithEmulator.value = event.isConnected;

        if (event.isConnected) {
            lastEmulatorConnectionErrorCode.value = null;
            lastEmulatorConnectionErrorDetail.value = null;
            return;
        }

        currentState.value = null;
        lastEmulatorConnectionErrorCode.value = event.errorCode;
        lastEmulatorConnectionErrorDetail.value = event.errorDetail;
    }

    function setInitialState(state: Events.StateDTO | null): void {
        if (!state) {
            currentState.value = null;
            return;
        }

        currentState.value = {
            player: state.player ? PlayerConverter.convert(state.player) : null,
            party: state.party ? PartyConverter.convert(state.party) : null,
            journal: state.journal ? JournalConverter.convert(state.journal) : null
        };
    }

    function syncPlayer(newPlayerDto: Events.PlayerDTO | null): void {
        const previousPlayer = currentState.value?.player;
        if (!previousPlayer || !newPlayerDto) {
            return;
        }

        PlayerSyncer.sync(previousPlayer, newPlayerDto);
    }
    
    function syncJournal(newJournalDto: Events.JournalDTO | null): void {
        const previousJournal = currentState.value?.journal;
        if (!previousJournal || !newJournalDto) {
            return;
        }

        JournalSyncer.sync(previousJournal, newJournalDto);
    }

    function syncParty(newPartyDto: Events.PartyDTO | null): void {
        const previousParty = currentState.value?.party;
        if (!previousParty || !newPartyDto) {
            return;
        }

        PartySyncer.sync(previousParty, newPartyDto);
    }

    return {
        isConnected,
        isConnectedWithBackend,
        isConnectedWithEmulator,
        backendProcessFailed,
        lastHubConnectionError,
        lastEmulatorConnectionErrorCode,
        lastEmulatorConnectionErrorDetail,
        setBackendProcessFailed,
        currentState,
        syncHubConnectionStatus,
        syncEmulatorConnectionStatus,
        setInitialState,
        syncPlayer,
        syncParty,
        syncJournal
    };
});

