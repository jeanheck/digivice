import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { State } from '../models/State'
import type * as Events from '../dtos/events.dto'
import { GameConverter } from '../converters/GameConverter'
import { DigimonUpdater } from '../updaters/DigimonUpdater'
import { PlayerUpdater } from '../updaters/PlayerUpdater'
import { ItemUpdater } from '../updaters/ItemUpdater'
import { JournalUpdater } from '../updaters/JournalUpdater'

export const useGameStore = defineStore('game', () => {
    const isConnected = ref(false)
    const gameState = ref<State | null>(null)

    // --- Status & Sync ---
    function updateConnectionStatus(event: Events.ConnectionStatusChangedDTO) {
        isConnected.value = event.isConnected
    }

    function updateInitialState(event: Events.InitialStateChangedDTO) {
        gameState.value = {
            player: GameConverter.toPlayerModel(event.initialState.player),
            party: GameConverter.toPartyModel(event.initialState.party),
            importantItems: GameConverter.toImportantItemsModel(event.initialState.importantItems),
            consumableItems: GameConverter.toConsumableItemsModel(event.initialState.consumableItems),
            journal: GameConverter.toJournalModel(event.initialState.journal)
        }
    }

    // --- Player Actions ---
    function updatePlayerBits(event: Events.PlayerBitsChangedDTO) {
        PlayerUpdater.updateBits(gameState.value, event)
    }

    function updatePlayerName(event: Events.PlayerNameChangedDTO) {
        PlayerUpdater.updateName(gameState.value, event)
    }

    function updatePlayerLocation(event: Events.PlayerLocationChangedDTO) {
        PlayerUpdater.updateLocation(gameState.value, event)
    }

    // --- Party & Digimon Actions ---
    function updatePartySlots(event: Events.PartySlotsChangedDTO) {
        DigimonUpdater.updateParty(gameState.value, event)
    }

    function updateDigimonVitals(event: Events.DigimonVitalsChangedDTO) {
        DigimonUpdater.updateVitals(gameState.value, event)
    }

    function updateDigimonExperience(event: Events.DigimonExperienceChangedDTO) {
        DigimonUpdater.updateExperience(gameState.value, event)
    }

    function updateDigimonLevel(event: Events.DigimonLevelChangedDTO) {
        DigimonUpdater.updateLevel(gameState.value, event)
    }

    function updateDigimonAttributes(event: Events.DigimonAttributesChangedDTO) {
        DigimonUpdater.updateAttributes(gameState.value, event)
    }

    function updateDigimonResistances(event: Events.DigimonResistancesChangedDTO) {
        DigimonUpdater.updateResistances(gameState.value, event)
    }

    function updateDigimonEquipments(event: Events.DigimonEquipmentsChangedDTO) {
        DigimonUpdater.updateEquipments(gameState.value, event)
    }

    function updateDigimonDigievolutions(event: Events.DigimonDigievolutionsChangedDTO) {
        DigimonUpdater.updateDigievolutions(gameState.value, event)
    }

    function updateDigimonActiveDigievolution(event: Events.DigimonActiveDigievolutionChangedDTO) {
        DigimonUpdater.updateActiveDigievolution(gameState.value, event)
    }

    // --- Items & Journal ---
    function updateImportantItems(event: Events.ImportantItemsChangedDTO) {
        ItemUpdater.updateImportantItems(gameState.value, event)
    }

    function updateJournal(event: Events.JournalChangedDTO) {
        JournalUpdater.updateJournal(gameState.value, event)
    }

    return {
        isConnected,
        gameState,
        updateConnectionStatus,
        updateInitialState,
        updatePlayerBits,
        updatePlayerName,
        updatePlayerLocation,
        updatePartySlots,
        updateDigimonVitals,
        updateDigimonExperience,
        updateDigimonLevel,
        updateDigimonAttributes,
        updateDigimonResistances,
        updateDigimonEquipments,
        updateDigimonDigievolutions,
        updateDigimonActiveDigievolution,
        updateImportantItems,
        updateJournal
    }
})
