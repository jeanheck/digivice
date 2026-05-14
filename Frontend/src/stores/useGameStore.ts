import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { State } from '../models/State'
import type * as Events from '../dtos/events.dto'
import { GameConverter } from '../converters/GameConverter'
import { BasicInfoConverter } from '../converters/BasicInfoConverter'
import { DigimonUpdater } from '../updaters/DigimonUpdater'
import { BasicInfoUpdater } from '../updaters/BasicInfoUpdater'
import { PlayerUpdater } from '../updaters/PlayerUpdater'
import { ItemUpdater } from '../updaters/ItemUpdater'
import { JournalUpdater } from '../updaters/JournalUpdater'
import { AttributesConverter } from '../converters/AttributesConverter'
import { ResistancesConverter } from '../converters/ResistancesConverter'
import { AttributesUpdater } from '../updaters/AttributesUpdater'
import { ResistancesUpdater } from '../updaters/ResistancesUpdater'

export const useGameStore = defineStore('game', () => {
    const isConnected = ref(false)
    const gameState = ref<State | null>(null)

    // --- Helpers ---
    function getDigimonOnPartySlot(slotIndex: number) {
        return gameState.value?.party?.slots[slotIndex] ?? null
    }

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
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex)
        if (!currentDigimon) return

        const newBasicInfo = BasicInfoConverter.convert(currentDigimon.basicInfo, {
            currentHP: event.currentHP,
            maxHP: event.maxHP,
            currentMP: event.currentMP,
            maxMP: event.maxMP
        })

        BasicInfoUpdater.update(currentDigimon, newBasicInfo)
    }

    function updateDigimonExperience(event: Events.DigimonExperienceChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex)
        if (!currentDigimon) return

        const newBasicInfo = BasicInfoConverter.convert(currentDigimon.basicInfo, {
            experience: event.experience
        })

        BasicInfoUpdater.update(currentDigimon, newBasicInfo)
    }

    function updateDigimonLevel(event: Events.DigimonLevelChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex)
        if (!currentDigimon) return

        const newBasicInfo = BasicInfoConverter.convert(currentDigimon.basicInfo, {
            level: event.level
        })

        BasicInfoUpdater.update(currentDigimon, newBasicInfo)
    }

    function updateDigimonAttributes(event: Events.DigimonAttributesChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex);
        if (!currentDigimon) {
            return;
        }

        const newAttributes = AttributesConverter.convert(currentDigimon.attributes, {
            strength: event.strength,
            defense: event.defense,
            spirit: event.spirit,
            wisdom: event.wisdom,
            speed: event.speed,
            charisma: event.charisma
        });

        AttributesUpdater.update(currentDigimon, newAttributes);
    }

    function updateDigimonResistances(event: Events.DigimonResistancesChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex);
        if (!currentDigimon) {
            return;
        }

        const newResistances = ResistancesConverter.convert(currentDigimon.resistances, {
            fire: event.fire,
            water: event.water,
            ice: event.ice,
            wind: event.wind,
            thunder: event.thunder,
            machine: event.machine,
            dark: event.dark
        });

        ResistancesUpdater.update(currentDigimon, newResistances);
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
