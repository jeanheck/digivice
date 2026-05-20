import { defineStore } from 'pinia';
import { ref, computed } from 'vue';
import type { State } from '../models/State';
import type * as Events from '../events/events.map';
import { PlayerConverter } from '../events/converters/player.converter';
import { PartyConverter } from '../events/converters/party.converter';
import { JournalConverter } from '../events/converters/journal.converter';
import { AreaInformationConverter } from '../events/converters/area-information.converter';
import { DigimonSlotConverter } from '../events/converters/digimon-slot.converter';
import { DigimonExperienceCalculator } from '../logic/DigimonExperienceCalculator';
import { DigievolutionRegistry } from '../logic/DigievolutionRegistry';
import { EquipmentsConverter } from '../events/converters/equipments.converter';
import { DigievolutionsConverter } from '../events/converters/digievolutions.converter';
import { AttributesStateManager } from '../stateManagers/AttributesStateManager';
import { ResistancesStateManager } from '../stateManagers/ResistancesStateManager';
import { PartyCalculator } from '../logic/PartyCalculator';
import { PlayerSyncer } from './syncers/player.syncer';

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
            importantItems: null,
            journal: JournalConverter.convert(state.journal)
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

            if (slotDto.digimonId === null || slotDto.digimon === null) {
                currentState.value!.party!.slots[index] = null;
            } else {
                const existingDigimon = currentState.value!.party!.slots[index];
                if (!existingDigimon || existingDigimon.activeDigievolutionId !== slotDto.digimonId) {
                    currentState.value!.party!.slots[index] = DigimonSlotConverter.convert(slotDto);
                } else {
                    const digimonDto = slotDto.digimon;
                    if (!digimonDto) {
                        return;
                    }
                    const digimon = existingDigimon;

                    if (digimonDto.level !== undefined) {
                        digimon.basicInfo.level = digimonDto.level;
                    }
                    if (digimonDto.experience !== undefined) {
                        digimon.basicInfo.experience = digimonDto.experience;
                    }

                    if (digimonDto.level !== undefined || digimonDto.experience !== undefined) {
                        digimon.basicInfo.experienceToReachNextLevel = DigimonExperienceCalculator.getRequiredExpForNextLevel(
                            digimon.basicInfo.name,
                            digimon.basicInfo.level
                        );
                        digimon.basicInfo.experiencePercentageToReachNextLevel = DigimonExperienceCalculator.getProgressPercentageForNextLevel(
                            digimon.basicInfo.name,
                            digimon.basicInfo.level,
                            digimon.basicInfo.experience
                        );
                    }

                    if (digimonDto.vitals) {
                        if (digimonDto.vitals.currentHP !== undefined) {
                            digimon.basicInfo.currentHP = digimonDto.vitals.currentHP;
                        }
                        if (digimonDto.vitals.maxHP !== undefined) {
                            digimon.basicInfo.maxHP = digimonDto.vitals.maxHP;
                        }
                        if (digimonDto.vitals.currentMP !== undefined) {
                            digimon.basicInfo.currentMP = digimonDto.vitals.currentMP;
                        }
                        if (digimonDto.vitals.maxMP !== undefined) {
                            digimon.basicInfo.maxMP = digimonDto.vitals.maxMP;
                        }
                    }

                    if (digimonDto.equipments) {
                        digimon.equipments = EquipmentsConverter.convert(digimonDto.equipments);
                    }

                    if (digimonDto.activeDigievolutionId !== undefined) {
                        digimon.activeDigievolutionId = digimonDto.activeDigievolutionId;
                        if (digimonDto.activeDigievolutionId !== null) {
                            digimon.basicInfo.name = DigievolutionRegistry.getDigievolutionNameById(
                                digimonDto.activeDigievolutionId
                            );
                        } else {
                            digimon.basicInfo.name = 'Unknown';
                        }
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

    function syncJournal(journal: Events.JournalDTO | null): void {
        if (!currentState.value) {
            return;
        }

        currentState.value.journal = JournalConverter.convert(journal);
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
