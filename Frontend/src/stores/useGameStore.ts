import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { State } from '../models/State'
import type * as Events from '../events/events.map'
import { PlayerConverter } from '../events/converters/player.converter'
import { PartyConverter } from '../events/converters/party.converter'
import { DigimonConverter } from '../events/converters/digimon.converter'
import { PartyUpdater } from '../updaters/PartyUpdater'
import { BasicInfoConverter } from '../converters/BasicInfoConverter'
import { DigievolutionsConverter } from '../events/converters/digievolutions.converter'
import { DigievolutionsUpdater } from '../updaters/DigievolutionsUpdater'
import { BasicInfoUpdater } from '../updaters/BasicInfoUpdater'
import { PlayerUpdater } from '../updaters/PlayerUpdater'
import { ImportantItemsConverter } from '../events/converters/important-items.converter'
import { ImportantItemsUpdater } from '../updaters/ImportantItemsUpdater'
import { JournalConverter } from '../events/converters/journal.converter'
import { JournalUpdater } from '../updaters/JournalUpdater'
import { EquipmentsConverter } from '../events/converters/equipments.converter'
import { EquipmentsUpdater } from '../updaters/EquipmentsUpdater'
import { ActiveDigievolutionConverter } from '../events/converters/active-digievolution.converter'
import { ActiveDigievolutionUpdater } from '../updaters/ActiveDigievolutionUpdater'
import { AttributesStateManager } from '../stateManagers/AttributesStateManager'
import { ResistancesStateManager } from '../stateManagers/ResistancesStateManager'
import { AreaInformationConverter } from '../events/converters/area-information.converter'

export const useGameStore = defineStore('game', () => {
    const isConnected = ref(false)
    const gameState = ref<State | null>(null)

    function getDigimonOnPartySlot(slotIndex: number) {
        return gameState.value?.party?.slots[slotIndex] ?? null
    }

    function updateConnectionStatus(event: Events.ConnectionStatusChangedDTO) {
        isConnected.value = event.isConnected
    }

    function updateInitialState(event: Events.InitialStateChangedDTO) {
        gameState.value = {
            player: PlayerConverter.convert(event.state.player),
            party: PartyConverter.convert(event.state.party),
            importantItems: ImportantItemsConverter.convert(event.state.importantItems),
            journal: JournalConverter.convert(event.state.journal),
            areaInformation: AreaInformationConverter.convert(event.state.player?.location)
        }
    }

    function updatePlayerBits(event: Events.PlayerBitsChangedDTO) {
        PlayerUpdater.updateBits(gameState.value, event)
    }

    function updatePlayerName(event: Events.PlayerNameChangedDTO) {
        PlayerUpdater.updateName(gameState.value, event)
    }

    function updatePlayerLocation(event: Events.PlayerLocationChangedDTO) {
        PlayerUpdater.updateLocation(gameState.value, event)
        if (gameState.value) {
            gameState.value.areaInformation = AreaInformationConverter.convert(event.location)
        }
    }

    function updatePartySlots(event: Events.PartySlotsChangedDTO) {
        if (!gameState.value?.party) return

        const slots = event.party.map(dto => DigimonConverter.convert(dto))
        PartyUpdater.update(gameState.value.party, slots)
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

        const newDigievolutions = DigievolutionsConverter.convert(event.digievolutions)
        DigievolutionsUpdater.update(currentDigimon, newDigievolutions)
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

    function updateImportantItems(event: Events.ImportantItemsChangedDTO) {
        if (!gameState.value) return

        const importantItems = ImportantItemsConverter.convert(event.importantItems)
        ImportantItemsUpdater.update(gameState.value, importantItems)
    }

    function updateJournal(event: Events.JournalChangedDTO) {
        if (!gameState.value) return

        const journal = JournalConverter.convert(event.journal)
        JournalUpdater.update(gameState.value, journal)
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
