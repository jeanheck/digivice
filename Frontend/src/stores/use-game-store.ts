import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { State } from '../models';
import type * as Events from '../events/events.map';
import { PlayerConverter } from '../events/converters/player.converter';
import { PartyConverter } from '../events/converters/party.converter';
import { JournalConverter } from '../events/converters/journal.converter';
import { AreaInformationConverter } from '../events/converters/area-information.converter';
import { PlayerSyncer } from './syncers/player.syncer';
import { JournalSyncer } from './syncers/journal.syncer';
import { PartySyncer } from './syncers/party.syncer';
import { PartyCalculator } from '../logic/PartyCalculator';

export const useGameStore = defineStore('game', () => {
    const isConnectedWithBackend = ref(false);
    const isConnectedWithEmulator = ref(false);
    const isConnected = computed(() => {
        return isConnectedWithBackend.value && isConnectedWithEmulator.value;
    });
    const currentState = ref<State | null>(null);
    const groupCharisma = computed(() => {
        return PartyCalculator.calculatePartyCharisma(currentState.value?.party?.slots ?? []);
    });

    function getDigimonOnPartySlot(slotIndex: number) {
        return currentState.value?.party?.slots[slotIndex] ?? null;
    }

    function syncHubConnectionStatus(event: { isConnected: boolean }): void {
        isConnectedWithBackend.value = event.isConnected;
    }

    function syncEmulatorConnectionStatus(event: { isConnected: boolean }): void {
        isConnectedWithEmulator.value = event.isConnected;
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
        currentState,
        groupCharisma,
        getDigimonOnPartySlot,
        syncHubConnectionStatus,
        syncEmulatorConnectionStatus,
        setInitialState,
        syncPlayer,
        syncParty,
        syncJournal
    };
});

