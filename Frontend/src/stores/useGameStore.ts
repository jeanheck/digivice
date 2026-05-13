import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { State } from '../types/backend'
import type * as Events from '../types/events'

export const useGameStore = defineStore('game', () => {
    const isConnected = ref(false)
    const gameState = ref<State | null>(null)

    // --- Status & Sync ---
    function updateConnectionStatus(event: Events.ConnectionStatusChanged) {
        isConnected.value = event.isConnected
    }

    function updateInitialState(event: Events.InitialStateChanged) {
        gameState.value = event.initialState
    }

    // --- Player Actions ---
    function updatePlayerBits(event: Events.PlayerBitsChanged) {
        if (gameState.value?.player) {
            gameState.value.player.bits = event.newBits
        }
    }

    function updatePlayerName(event: Events.PlayerNameChanged) {
        if (gameState.value?.player) {
            gameState.value.player.name = event.newName
        }
    }

    function updatePlayerLocation(event: Events.PlayerLocationChanged) {
        if (gameState.value?.player) {
            gameState.value.player.mapId = event.location
        }
    }

    // --- Party & Digimon Actions ---
    function updatePartySlots(event: Events.PartySlotsChanged) {
        if (gameState.value?.party) {
            gameState.value.party.slots = event.newParty
        }
    }

    function updateDigimonVitals(event: Events.DigimonVitalsChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            const digimon = gameState.value.party.slots[event.partySlotIndex]!
            digimon.basicInfo.currentHP = event.currentHP
            digimon.basicInfo.maxHP = event.maxHP
            digimon.basicInfo.currentMP = event.currentMP
            digimon.basicInfo.maxMP = event.maxMP
        }
    }

    function updateDigimonExperience(event: Events.DigimonExperienceChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            const digimon = gameState.value.party.slots[event.partySlotIndex]!
            digimon.basicInfo.level = event.level
            digimon.basicInfo.experience = event.currentEXP
        }
    }

    function updateDigimonLevel(event: Events.DigimonLevelChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.basicInfo.level = event.newLevel
        }
    }

    function updateDigimonAttributes(event: Events.DigimonAttributesChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            const attrs = gameState.value.party.slots[event.partySlotIndex]!.attributes
            attrs.strength = event.strength
            attrs.defense = event.defense
            attrs.spirit = event.spirit
            attrs.wisdom = event.wisdom
            attrs.speed = event.speed
            attrs.charisma = event.charisma
        }
    }

    function updateDigimonResistances(event: Events.DigimonResistancesChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            const res = gameState.value.party.slots[event.partySlotIndex]!.resistances
            res.fire = event.fire
            res.water = event.water
            res.ice = event.ice
            res.wind = event.wind
            res.thunder = event.thunder
            res.machine = event.machine
            res.dark = event.dark
        }
    }

    function updateDigimonEquipments(event: Events.DigimonEquipmentsChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.equipments = event.equipments
        }
    }

    function updateDigimonDigievolutions(event: Events.DigimonDigievolutionsChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.equippedDigievolutions = event.equippedDigievolutions
        }
    }

    function updateDigimonActiveDigievolution(event: Events.DigimonActiveDigievolutionChanged) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.activeDigievolutionId = event.activeDigievolutionId
        }
    }

    function updateDigimonDigievolutionLevel(event: Events.DigimonDigievolutionLevelChanged) {
        console.log(`[LEVEL UP!] Digimon in slot ${event.partySlotIndex}'s Digievolution (ID ${event.digievolutionId}) reached Level ${event.newLevel}!`)
    }

    // --- Items & Journal ---
    function updateImportantItems(event: Events.ImportantItemsChanged) {
        if (gameState.value) {
            gameState.value.importantItems = event.importantItems
        }
    }

    function updateJournal(event: Events.JournalChanged) {
        if (gameState.value) {
            gameState.value.journal = event.journal
        }
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
        updateDigimonDigievolutionLevel,
        updateImportantItems,
        updateJournal
    }
})
