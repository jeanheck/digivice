import { signalRService } from '../services/SignalRService'
import { useGameStore } from '../stores/useGameStore'

/**
 * Connects SignalR events to Pinia Stores actions.
 * This file serves as the "glue" between the network service and the application state.
 */
export function initSignalRHandlers() {
    const store = useGameStore()

    signalRService.on('ConnectionStatusChanged', (event) => {
        store.setConnectionStatus(event.isConnected)
    })

    signalRService.on('InitialStateSync', (event) => {
        store.setInitialState(event.initialState)
    })

    signalRService.on('PlayerBitsChanged', (event) => {
        store.updateBits(event.newBits)
    })

    signalRService.on('PlayerNameChanged', (event) => {
        store.updatePlayerName(event.newName)
    })

    signalRService.on('PartySlotsChanged', (event) => {
        store.updatePartySlots(event.newParty)
    })

    signalRService.on('DigimonVitalsChanged', (event) => {
        store.updateDigimonVitals(
            event.partySlotIndex,
            event.currentHP,
            event.maxHP,
            event.currentMP,
            event.maxMP
        )
    })

    signalRService.on('DigimonXpGained', (event) => {
        store.updateDigimonExperience(event.partySlotIndex, event.level, event.currentEXP)
    })

    signalRService.on('DigimonLevelUp', (event) => {
        store.updateDigimonLevel(event.partySlotIndex, event.newLevel)
    })

    signalRService.on('DigimonAttributesChanged', (event) => {
        store.updateDigimonAttributes(
            event.partySlotIndex,
            event.strength,
            event.defense,
            event.spirit,
            event.wisdom,
            event.speed,
            event.charisma
        )
    })

    signalRService.on('DigimonResistancesChanged', (event) => {
        store.updateDigimonResistances(
            event.partySlotIndex,
            event.fire,
            event.water,
            event.ice,
            event.wind,
            event.thunder,
            event.machine,
            event.dark
        )
    })

    signalRService.on('DigimonEquipmentsChanged', (event) => {
        store.updateDigimonEquipments(event.partySlotIndex, event.equipments)
    })

    signalRService.on('DigimonDigievolutionsChanged', (event) => {
        store.updateDigimonDigievolutions(event.partySlotIndex, event.equippedDigievolutions)
    })

    signalRService.on('DigimonActiveDigievolutionChanged', (event) => {
        store.updateActiveDigievolutionId(event.partySlotIndex, event.activeDigievolutionId)
    })

    signalRService.on('DigimonDigievolutionLevelUp', (event) => {
        store.notifyDigievolutionLevelUp(
            event.partySlotIndex,
            event.digievolutionId,
            event.oldLevel,
            event.newLevel
        )
    })

    signalRService.on('ImportantItemsChanged', (event) => {
        store.updateImportantItems(event.importantItems)
    })

    signalRService.on('JournalChanged', (event) => {
        store.updateJournal(event.journal)
    })

    signalRService.on('LocationChanged', (event) => {
        store.updateLocation(event.location)
    })
}
