import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { State } from '../models/State';
import type * as Events from '../events/events.map';
import { PlayerConverter } from '../events/converters/player.converter';
import { PartyConverter } from '../events/converters/party.converter';
import { JournalConverter } from '../events/converters/journal.converter';
import { AreaInformationConverter } from '../events/converters/area-information.converter';
import { DigimonSlotConverter } from '../events/converters/digimon-slot.converter';
import { DigimonConverter } from '../events/converters/digimon.converter';
import { EquipmentsConverter } from '../events/converters/equipments.converter';
import { DigievolutionsConverter } from '../events/converters/digievolutions.converter';
import { AttributesStateManager } from '../stateManagers/AttributesStateManager';
import { ResistancesStateManager } from '../stateManagers/ResistancesStateManager';
import { PartyCalculator } from '../logic/PartyCalculator';
import { PlayerSyncer } from './syncers/player.syncer';
import { JournalSyncer } from './syncers/journal.syncer';

export const useGameStore = defineStore('game', () => {
    const isConnectedWithBackend = ref(false);
    const isConnectedWithEmulator = ref(false);
    const isConnected = computed(() => {
        return isConnectedWithBackend.value && isConnectedWithEmulator.value;
    });
    const currentState = ref<State | null>(null);
    const areaInformation = computed(() => {
        return AreaInformationConverter.convert(currentState.value?.player?.location);
    });

    function getDigimonOnPartySlot(slotIndex: number) {
        return currentState.value?.party?.slots[slotIndex] ?? null;
    }

    function syncHubConnectionStatus(event: { isConnected: boolean }): void {
        isConnectedWithBackend.value = event.isConnected;
    }

    function syncEmulatorConnectionStatus(event: Events.EmulatorConnectionStatusChangedDTO): void {
        isConnectedWithEmulator.value = event.isConnected;
    }

    function setInitialState(state: Events.StateDTO | null): void {
        if (!state) {
            currentState.value = null;
            return;
        }

        currentState.value = {
            player: state.player ? PlayerConverter.convert(state.player) : null,
            party: PartyConverter.convert(state.party),
            journal: state.journal ? JournalConverter.convert(state.journal) : null
        };
    }

    function syncPlayer(playerDto: Events.PlayerDTO | null): void {
        const player = currentState.value?.player;
        if (!player || !playerDto) {
            return;
        }

        PlayerSyncer.sync(player, playerDto);
    }

    function syncParty(partyDto: Events.PartyDTO | null): void {
        if (!currentState.value?.party || !partyDto?.slots) {
            return;
        }

        partyDto.slots.forEach((slotDto) => {
            if (!slotDto) {
                return;
            }

            const index = slotDto.index;
            if (index < 0 || index >= currentState.value!.party!.slots.length) {
                return;
            }

            if (slotDto.digimonId === undefined || slotDto.digimonId === null || !slotDto.digimon) {
                currentState.value!.party!.slots[index] = {
                    index,
                    digimonId: null,
                    digimon: null
                };
            } else {
                const existingSlot = currentState.value!.party!.slots[index];
                if (!existingSlot || existingSlot.digimonId !== slotDto.digimonId) {
                    currentState.value!.party!.slots[index] = DigimonSlotConverter.convert(slotDto)!;
                } else {
                    const digimonDto = slotDto.digimon;
                    if (!digimonDto) {
                        return;
                    }
                    const digimon = existingSlot.digimon;
                    if (!digimon) {
                        existingSlot.digimon = DigimonConverter.convert(digimonDto);
                        return;
                    }

                    if (digimonDto.level !== undefined) {
                        digimon.level = digimonDto.level;
                    }
                    if (digimonDto.experience !== undefined) {
                        digimon.experience = digimonDto.experience;
                    }

                    if (digimonDto.vitals) {
                        if (digimonDto.vitals.currentHP !== undefined) {
                            digimon.vitals.currentHP = digimonDto.vitals.currentHP;
                        }
                        if (digimonDto.vitals.maxHP !== undefined) {
                            digimon.vitals.maxHP = digimonDto.vitals.maxHP;
                        }
                        if (digimonDto.vitals.currentMP !== undefined) {
                            digimon.vitals.currentMP = digimonDto.vitals.currentMP;
                        }
                        if (digimonDto.vitals.maxMP !== undefined) {
                            digimon.vitals.maxMP = digimonDto.vitals.maxMP;
                        }
                    }

                    if (digimonDto.equipments) {
                        digimon.equipments = EquipmentsConverter.convert(digimonDto.equipments)!;
                    }

                    if (digimonDto.activeDigievolutionId !== undefined) {
                        digimon.activeDigievolutionId = digimonDto.activeDigievolutionId;
                    }

                    if (digimonDto.digievolutions) {
                        digimon.digievolutions = DigievolutionsConverter.convert(digimonDto.digievolutions);
                    }

                    if (digimonDto.attributes) {
                        AttributesStateManager.refresh(digimon, digimonDto.attributes);
                    } else if (digimonDto.equipments || digimonDto.activeDigievolutionId !== undefined) {
                        AttributesStateManager.refresh(digimon);
                    }

                    if (digimonDto.resistances) {
                        ResistancesStateManager.refresh(digimon, digimonDto.resistances);
                    } else if (digimonDto.equipments || digimonDto.activeDigievolutionId !== undefined) {
                        ResistancesStateManager.refresh(digimon);
                    }
                }
            }
        });

        currentState.value.party.groupCharisma = PartyCalculator.calculateGroupCharisma(
            currentState.value.party.slots
        );
    }

    function syncJournal(journalDto: Events.JournalDTO | null): void {
        const journal = currentState.value?.journal;
        if (!journal || !journalDto) {
            return;
        }

        JournalSyncer.sync(journal, journalDto);
    }

    return {
        isConnected,
        isConnectedWithBackend,
        isConnectedWithEmulator,
        currentState,
        areaInformation,
        getDigimonOnPartySlot,
        syncHubConnectionStatus,
        syncEmulatorConnectionStatus,
        setInitialState,
        syncPlayer,
        syncParty,
        syncJournal
    };
});
