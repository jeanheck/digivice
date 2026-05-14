import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { State } from '../models/State'
import type * as Events from '../dtos/events.dto'
import { GameConverter } from '../converters/GameConverter'
import { BasicInfoConverter } from '../converters/BasicInfoConverter'
import { PartySlotsConverter } from '../converters/PartySlotsConverter'
import { PartySlotsUpdater } from '../updaters/PartySlotsUpdater'
import { EquippedDigievolutionsConverter } from '../converters/EquippedDigievolutionsConverter'
import { EquippedDigievolutionsUpdater } from '../updaters/EquippedDigievolutionsUpdater'
import { BasicInfoUpdater } from '../updaters/BasicInfoUpdater'
import { PlayerUpdater } from '../updaters/PlayerUpdater'
import { ImportantItemsConverter } from '../converters/ImportantItemsConverter'
import { ImportantItemsUpdater } from '../updaters/ImportantItemsUpdater'
import { JournalUpdater } from '../updaters/JournalUpdater'
import { EquipmentsConverter } from '../converters/EquipmentsConverter'
import { EquipmentsUpdater } from '../updaters/EquipmentsUpdater'
import { ActiveDigievolutionConverter } from '../converters/ActiveDigievolutionConverter'
import { ActiveDigievolutionUpdater } from '../updaters/ActiveDigievolutionUpdater'
import { AttributesStateManager } from '../stateManagers/AttributesStateManager'
import { ResistancesStateManager } from '../stateManagers/ResistancesStateManager'

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
            player: GameConverter.toPlayerModel(event.state.player),
            party: GameConverter.toPartyModel(event.state.party),
            importantItems: ImportantItemsConverter.convert(event.state.importantItems),
            consumableItems: GameConverter.toConsumableItemsModel(event.state.consumableItems),
            journal: GameConverter.toJournalModel(event.state.journal)
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
        if (!gameState.value?.party) return

        const slots = PartySlotsConverter.convert(event.party)
        PartySlotsUpdater.update(gameState.value.party, slots)
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

        AttributesStateManager.refresh(currentDigimon, event);
    }

    function updateDigimonResistances(event: Events.DigimonResistancesChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex);
        if (!currentDigimon) {
            return;
        }

        ResistancesStateManager.refresh(currentDigimon, event);
    }

    function updateDigimonEquipments(event: Events.DigimonEquipmentsChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex);
        if (!currentDigimon) {
            return;
        }

        const newEquipments = EquipmentsConverter.convert(event.equipments);

        EquipmentsUpdater.update(currentDigimon, newEquipments);

        AttributesStateManager.refresh(currentDigimon);
        ResistancesStateManager.refresh(currentDigimon);
    }

    function updateDigimonDigievolutions(event: Events.DigimonDigievolutionsChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex)
        if (!currentDigimon) return

        const newDigievolutions = EquippedDigievolutionsConverter.convert(event.equippedDigievolutions)
        EquippedDigievolutionsUpdater.update(currentDigimon, newDigievolutions)
    }

    function updateDigimonActiveDigievolution(event: Events.DigimonActiveDigievolutionChangedDTO) {
        const currentDigimon = getDigimonOnPartySlot(event.partySlotIndex);
        if (!currentDigimon) {
            return;
        }

        const newActiveDigievolutionId = ActiveDigievolutionConverter.convert(event.activeDigievolutionId);
        ActiveDigievolutionUpdater.update(currentDigimon, newActiveDigievolutionId);

        AttributesStateManager.refresh(currentDigimon);
        ResistancesStateManager.refresh(currentDigimon);
    }

    // --- Items & Journal ---
    function updateImportantItems(event: Events.ImportantItemsChangedDTO) {
        if (!gameState.value) return

        const importantItems = ImportantItemsConverter.convert(event.importantItems)
        ImportantItemsUpdater.update(gameState.value, importantItems)
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
