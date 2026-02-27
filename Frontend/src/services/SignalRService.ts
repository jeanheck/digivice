import * as signalR from '@microsoft/signalr'
import { useGameStore } from '../stores/useGameStore'
import type { State } from '../types/backend'

class SignalRService {
    private connection: signalR.HubConnection | null = null

    public async startConnection() {
        const store = useGameStore()

        this.connection = new signalR.HubConnectionBuilder()
            .withUrl('/gamehub') // Will be proxied by Vite
            .withAutomaticReconnect()
            .build()

        this.connection.onreconnecting(() => {
            console.warn('SignalR reconnecting...')
            store.setConnectionStatus(false)
        })

        this.connection.onreconnected(() => {
            console.log('SignalR reconnected.')
            store.setConnectionStatus(true)
        })

        this.connection.onclose(() => {
            console.error('SignalR connection closed.')
            store.setConnectionStatus(false)
        })

        this.registerEvents(store)

        try {
            await this.connection.start()
            console.log('SignalR Connected to GameHub.')
            store.setConnectionStatus(true)
        } catch (err) {
            console.error('SignalR Connection Error: ', err)
            store.setConnectionStatus(false)
        }
    }

    private registerEvents(store: ReturnType<typeof useGameStore>) {
        if (!this.connection) return

        this.connection.on('ConnectionStatusChanged', (event: { isConnected: boolean }) => {
            console.log('Hub: ConnectionStatusChanged', event)
            store.setConnectionStatus(event.isConnected)
        })

        this.connection.on('InitialStateSync', (event: { initialState: State }) => {
            console.log('Hub: InitialStateSync', event)
            store.setInitialState(event.initialState)
        })

        this.connection.on('PlayerBitsChanged', (event: { newBits: number }) => {
            console.log('Hub: PlayerBitsChanged', event)
            store.updateBits(event.newBits)
        })

        this.connection.on('PartySlotsChanged', (event: { newParty: (import('../types/backend').Digimon | null)[] }) => {
            console.log('Hub: PartySlotsChanged', event)
            store.updatePartySlots(event.newParty)
        })

        this.connection.on('DigimonVitalsChanged', (event: { partySlotIndex: number, currentHP: number, maxHP: number, currentMP: number, maxMP: number }) => {
            console.log('Hub: DigimonVitalsChanged', event)
            store.updateDigimonVitals(event.partySlotIndex, event.currentHP, event.maxHP, event.currentMP, event.maxMP)
        })

        this.connection.on('DigimonXpGained', (event: { partySlotIndex: number, level: number, currentEXP: number }) => {
            console.log('Hub: DigimonXpGained', event)
            store.updateDigimonExperience(event.partySlotIndex, event.level, event.currentEXP)
        })

        this.connection.on('DigimonLevelUp', (event: { partySlotIndex: number, oldLevel: number, newLevel: number }) => {
            console.log('Hub: DigimonLevelUp', event)
            store.updateDigimonLevel(event.partySlotIndex, event.newLevel)
        })

        this.connection.on('DigimonAttributesChanged', (event: { partySlotIndex: number, strength: number, defense: number, spirit: number, wisdom: number, speed: number, charisma: number }) => {
            console.log('Hub: DigimonAttributesChanged', event)
            store.updateDigimonAttributes(event.partySlotIndex, event.strength, event.defense, event.spirit, event.wisdom, event.speed, event.charisma)
        })

        this.connection.on('DigimonResistancesChanged', (event: { partySlotIndex: number, fire: number, water: number, ice: number, wind: number, thunder: number, machine: number, dark: number }) => {
            console.log('Hub: DigimonResistancesChanged', event)
            store.updateDigimonResistances(event.partySlotIndex, event.fire, event.water, event.ice, event.wind, event.thunder, event.machine, event.dark)
        })

        this.connection.on('DigimonEquipmentsChanged', (event: { partySlotIndex: number, equipments: import('../types/backend').Equipments }) => {
            console.log('Hub: DigimonEquipmentsChanged', event)
            store.updateDigimonEquipments(event.partySlotIndex, event.equipments)
        })

        this.connection.on('DigimonDigievolutionsChanged', (event: { partySlotIndex: number, equippedDigievolutions: (import('../types/backend').Digievolution | null)[] }) => {
            console.log('Hub: DigimonDigievolutionsChanged', event)
            store.updateDigimonDigievolutions(event.partySlotIndex, event.equippedDigievolutions)
        })

        this.connection.on('DigimonDigievolutionLevelUp', (event: { partySlotIndex: number, digievolutionId: number, oldLevel: number, newLevel: number }) => {
            console.log('Hub: DigimonDigievolutionLevelUp', event)
            store.notifyDigievolutionLevelUp(event.partySlotIndex, event.digievolutionId, event.oldLevel, event.newLevel)
        })

        this.connection.on('ImportantItemsChanged', (event: { importantItems: Record<string, boolean> }) => {
            console.log('Hub: ImportantItemsChanged', event)
            store.updateImportantItems(event.importantItems)
        })

        this.connection.on('JournalChanged', (event: { journal: import('../types/backend').Journal | null }) => {
            console.log('Hub: JournalChanged', event)
            store.updateJournal(event.journal)
        })

        this.connection.on('LocationChanged', (event: { location: string }) => {
            console.log('Hub: LocationChanged', event)
            store.updateLocation(event.location)
        })
    }

    public get isConnected() {
        return this.connection?.state === signalR.HubConnectionState.Connected
    }
}

export const signalRService = new SignalRService()
