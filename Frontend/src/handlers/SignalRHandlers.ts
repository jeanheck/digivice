import { signalRService } from '../services/SignalRService'
import { useGameStore } from '../stores/useGameStore'
import { signalRLogger } from '../utils/Logger'

/**
 * Conecta os 5 macro-eventos do SignalR com as respectivas ações da Pinia Store.
 */
export function initSignalRHandlers() {
    const store = useGameStore()

    // 1. Assina ConnectionStatusChanged
    signalRService.on('ConnectionStatusChanged', (data) => {
        store.updateConnectionStatus(data)
    })

    // 2. Assina InitialState (Backend) -> Mapeia para updateInitialState da Store
    signalRService.on('InitialState', (data) => {
        // Envolve em um objeto contendo a propriedade 'state' para conformidade com a assinatura
        store.updateInitialState({ 
            state: {
                player: data.player,
                party: data.party,
                importantItems: null,
                journal: data.journal
            } 
        })
    })

    // 3. Assina PlayerChanged -> Mapeia para updatePlayer na Store
    signalRService.on('PlayerChanged', (data) => {
        if (typeof (store as any).updatePlayer === 'function') {
            (store as any).updatePlayer(data)
        }
    })

    // 4. Assina PartyChanged -> Mapeia para updateParty na Store
    signalRService.on('PartyChanged', (data) => {
        if (typeof (store as any).updateParty === 'function') {
            (store as any).updateParty(data)
        }
    })

    // 5. Assina JournalChanged -> Mapeia para updateJournal na Store
    signalRService.on('JournalChanged', (data) => {
        store.updateJournal({ journal: data })
    })

    signalRLogger.debug('SignalR Handlers successfully initialized for the 5 macro-events.')
}
