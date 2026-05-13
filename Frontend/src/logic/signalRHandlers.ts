import { signalRService } from '../services/SignalRService'
import { useGameStore } from '../stores/useGameStore'
import type { GameEventMap } from '../types/events'

/**
 * Connects SignalR events to Pinia Stores actions using a naming convention:
 * Backend Event: [Entity][Property]Changed
 * Frontend Store Action: update[Entity][Property]
 * 
 * This file is now fully automated and uses GameEventMap for type consistency.
 */
export function initSignalRHandlers() {
    const store = useGameStore()

    // 1. Discover all store actions starting with 'update'
    const storeActions = Object.keys(store).filter(key => key.startsWith('update'))

    // 2. Automatically map each action to its corresponding backend event
    storeActions.forEach(actionName => {
        // Convention: 'updatePlayerBits' -> 'PlayerBitsChanged'
        const eventName = (actionName.replace('update', '') + 'Changed') as keyof GameEventMap
        
        // signalRService.on is now strongly typed!
        signalRService.on(eventName, (data) => {
            // Call the store action passing the typed event payload
            const action = (store as any)[actionName]
            if (typeof action === 'function') {
                action(data)
            }
        })
    })

    console.log(`[SignalR] Auto-registered ${storeActions.length} event handlers with strong typing support.`)
}
