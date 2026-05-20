import { signalRService } from './signalr.service';
import { useGameStore } from '../stores/useGameStore';
import { signalRLogger } from './logger';

/**
 * Initializes the SignalR handlers to bridge incoming backend events with the Pinia store actions.
 */
export function initializeSignalRHandlers(): void {
    const store = useGameStore();

    signalRService.on('HubConnectionStatusChanged', (data) => {
        store.updateHubConnectionStatus(data);
    });

    signalRService.on('EmulatorConnectionStatusChanged', (data) => {
        store.updateEmulatorConnectionStatus(data);
    });

    signalRService.on('InitialState', (data) => {
        // Wrap the payload in an object containing the 'state' property to match the store's action signature
        store.updateInitialState({
            state: {
                player: data.player,
                party: data.party,
                importantItems: null,
                journal: data.journal
            }
        });
    });

    signalRService.on('PlayerChanged', (data) => {
        if (typeof (store as any).updatePlayer === 'function') {
            (store as any).updatePlayer(data);
        }
    });

    signalRService.on('PartyChanged', (data) => {
        if (typeof (store as any).updateParty === 'function') {
            (store as any).updateParty(data);
        }
    });

    signalRService.on('JournalChanged', (data) => {
        store.updateJournal({ journal: data });
    });

    signalRLogger.debug('SignalR Handlers successfully initialized for the 5 macro-events.');
}
