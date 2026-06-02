import { signalRService } from './signalr.service';
import { useGameStore } from '../stores/use-game-store';
import { signalRLogger } from './logger';
import { parseConnectionStatus } from './connection-status';

/**
 * Initializes the SignalR handlers to bridge incoming backend events with the Pinia store actions.
 */
export function initializeSignalRHandlers(): void {
    const store = useGameStore();

    signalRService.on('HubConnectionStatusChanged', (data) => {
        store.syncHubConnectionStatus(data);
    });

    signalRService.on('EmulatorConnectionStatusChanged', (data) => {
        store.syncEmulatorConnectionStatus(parseConnectionStatus(data));
    });

    signalRService.on('InitialState', (data) => {
        store.setInitialState(data);
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
