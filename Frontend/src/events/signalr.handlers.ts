import { signalRService } from './signalr.service';
import { useGameStore } from '../stores/useGameStore';
import { signalRLogger } from './logger';

/**
 * Initializes the SignalR handlers to bridge incoming backend events with the Pinia store actions.
 */
export function initializeSignalRHandlers(): void {
    const store = useGameStore();

    signalRService.on('HubConnectionStatusChanged', (data) => {
        store.syncHubConnectionStatus(data);
    });

    signalRService.on('EmulatorConnectionStatusChanged', (data) => {
        store.syncEmulatorConnectionStatus(data);
    });

    signalRService.on('InitialState', (data) => {
        store.syncInitialState(data);
    });

    signalRService.on('PlayerChanged', (data) => {
        store.syncPlayer(data);
    });

    signalRService.on('PartyChanged', (data) => {
        store.syncParty(data);
    });

    signalRService.on('JournalChanged', (data) => {
        store.syncJournal(data);
    });

    signalRLogger.debug('SignalR Handlers successfully initialized.');
}
