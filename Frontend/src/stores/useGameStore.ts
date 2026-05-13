import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Digimon, State, ImportantItems, Journal } from '../types/backend'

export const useGameStore = defineStore('game', () => {
    const isConnected = ref(false)
    const gameState = ref<State | null>(null)

    // --- Status & Sync ---
    function updateConnectionStatus(event: { isConnected: boolean }) {
        isConnected.value = event.isConnected
    }

    function updateInitialState(event: { initialState: State }) {
        gameState.value = event.initialState
    }

    // --- Player Actions ---
    function updatePlayerBits(event: { newBits: number }) {
        if (gameState.value?.player) {
            gameState.value.player.bits = event.newBits
        }
    }

    function updatePlayerName(event: { newName: string }) {
        if (gameState.value?.player) {
            gameState.value.player.name = event.newName
        }
    }

    function updatePlayerLocation(event: { location: string }) {
        if (gameState.value?.player) {
            gameState.value.player.mapId = event.location
        }
    }

    // --- Party & Digimon Actions ---
    function updatePartySlots(event: { newParty: (Digimon | null)[] }) {
        if (gameState.value?.party) {
            gameState.value.party.slots = event.newParty
        }
    }

    function updateDigimonVitals(event: { partySlotIndex: number, currentHP: number, maxHP: number, currentMP: number, maxMP: number }) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            const digimon = gameState.value.party.slots[event.partySlotIndex]!
            digimon.basicInfo.currentHP = event.currentHP
            digimon.basicInfo.maxHP = event.maxHP
            digimon.basicInfo.currentMP = event.currentMP
            digimon.basicInfo.maxMP = event.maxMP
        }
    }

    function updateDigimonExperience(event: { partySlotIndex: number, level: number, currentEXP: number }) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            const digimon = gameState.value.party.slots[event.partySlotIndex]!
            digimon.basicInfo.level = event.level
            digimon.basicInfo.experience = event.currentEXP
        }
    }

    function updateDigimonLevel(event: { partySlotIndex: number, oldLevel: number, newLevel: number }) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.basicInfo.level = event.newLevel
        }
    }

    function updateDigimonAttributes(event: { partySlotIndex: number, strength: number, defense: number, spirit: number, wisdom: number, speed: number, charisma: number }) {
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

    function updateDigimonResistances(event: { partySlotIndex: number, fire: number, water: number, ice: number, wind: number, thunder: number, machine: number, dark: number }) {
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

    function updateDigimonEquipments(event: { partySlotIndex: number, equipments: Digimon['equipments'] }) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.equipments = event.equipments
        }
    }

    function updateDigimonDigievolutions(event: { partySlotIndex: number, equippedDigievolutions: Digimon['equippedDigievolutions'] }) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.equippedDigievolutions = event.equippedDigievolutions
        }
    }

    function updateDigimonActiveDigievolution(event: { partySlotIndex: number, activeDigievolutionId: number | null }) {
        if (gameState.value?.party?.slots[event.partySlotIndex]) {
            gameState.value.party.slots[event.partySlotIndex]!.activeDigievolutionId = event.activeDigievolutionId
        }
    }

    function updateDigimonDigievolutionLevel(event: { partySlotIndex: number, digievolutionId: number, oldLevel: number, newLevel: number }) {
        console.log(`[LEVEL UP!] Digimon in slot ${event.partySlotIndex}'s Digievolution (ID ${event.digievolutionId}) reached Level ${event.newLevel}!`)
    }

    // --- Items & Journal ---
    function updateImportantItems(event: { importantItems: ImportantItems | null }) {
        if (gameState.value) {
            gameState.value.importantItems = event.importantItems
        }
    }

    function updateJournal(event: { journal: Journal | null }) {
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
