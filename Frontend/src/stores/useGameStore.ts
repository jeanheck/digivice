import { defineStore } from 'pinia'
import { ref } from 'vue'
import type { Digimon, State } from '../types/backend'

export const useGameStore = defineStore('game', () => {
    const isConnected = ref(false)
    const gameState = ref<State | null>(null)

    function setConnectionStatus(status: boolean) {
        isConnected.value = status
    }

    function setInitialState(state: State) {
        gameState.value = state
    }

    function updateBits(newBits: number) {
        if (gameState.value?.player) {
            gameState.value.player.bits = newBits
        }
    }

    function updatePartySlots(newSlots: (Digimon | null)[]) {
        if (gameState.value?.party) {
            // preserve reactivity
            gameState.value.party.slots = newSlots
        }
    }

    function updateDigimonVitals(slotIndex: number, currentHP: number, maxHP: number, currentMP: number, maxMP: number) {
        if (gameState.value?.party?.slots[slotIndex]) {
            const digimon = gameState.value.party.slots[slotIndex]!
            digimon.basicInfo.currentHP = currentHP
            digimon.basicInfo.maxHP = maxHP
            digimon.basicInfo.currentMP = currentMP
            digimon.basicInfo.maxMP = maxMP
        }
    }

    function updateDigimonExperience(slotIndex: number, level: number, currentExp: number) {
        if (gameState.value?.party?.slots[slotIndex]) {
            const digimon = gameState.value.party.slots[slotIndex]!
            digimon.basicInfo.level = level
            digimon.basicInfo.experience = currentExp
        }
    }

    function updateDigimonLevel(slotIndex: number, newLevel: number) {
        if (gameState.value?.party?.slots[slotIndex]) {
            gameState.value.party.slots[slotIndex]!.basicInfo.level = newLevel
        }
    }

    function updateDigimonAttributes(slotIndex: number, strength: number, defense: number, spirit: number, wisdom: number, speed: number, charisma: number) {
        if (gameState.value?.party?.slots[slotIndex]) {
            const attrs = gameState.value.party.slots[slotIndex]!.attributes
            attrs.strength = strength
            attrs.defense = defense
            attrs.spirit = spirit
            attrs.wisdom = wisdom
            attrs.speed = speed
            attrs.charisma = charisma
        }
    }

    function updateDigimonResistances(slotIndex: number, fire: number, water: number, ice: number, wind: number, thunder: number, machine: number, dark: number) {
        if (gameState.value?.party?.slots[slotIndex]) {
            const res = gameState.value.party.slots[slotIndex]!.resistances
            res.fire = fire
            res.water = water
            res.ice = ice
            res.wind = wind
            res.thunder = thunder
            res.machine = machine
            res.dark = dark
        }
    }

    function updateDigimonEquipments(slotIndex: number, newEquips: Digimon['equipments']) {
        if (gameState.value?.party?.slots[slotIndex]) {
            gameState.value.party.slots[slotIndex]!.equipments = newEquips
        }
    }

    function updateDigimonDigievolutions(slotIndex: number, newDigievolutions: Digimon['equippedDigievolutions']) {
        if (gameState.value?.party?.slots[slotIndex]) {
            gameState.value.party.slots[slotIndex]!.equippedDigievolutions = newDigievolutions
        }
    }

    function notifyDigievolutionLevelUp(slotIndex: number, digievolutionId: number, oldLevel: number, newLevel: number) {
        console.log(`[LEVEL UP!] Digimon in slot ${slotIndex}'s Digievolution (ID ${digievolutionId}) reached Level ${newLevel}!`)
    }

    return {
        isConnected,
        gameState,
        setConnectionStatus,
        setInitialState,
        updateBits,
        updatePartySlots,
        updateDigimonVitals,
        updateDigimonExperience,
        updateDigimonLevel,
        updateDigimonAttributes,
        updateDigimonResistances,
        updateDigimonEquipments,
        updateDigimonDigievolutions,
        notifyDigievolutionLevelUp
    }
})
