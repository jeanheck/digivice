import type { State } from '../models/State';
import type * as Events from '../dtos/events.dto';
import { GameConverter } from '../converters/GameConverter';

/**
 * DigimonUpdater
 * Contains the logic for mutating Digimon-related state.
 */
export class DigimonUpdater {

    public static updateParty(state: State | null, event: Events.PartySlotsChangedDTO) {
        if (state?.party) {
            state.party.slots = event.newParty.map(slotDto => 
                GameConverter.toDigimonModel(slotDto)
            );
        }
    }

    public static updateVitals(state: State | null, event: Events.DigimonVitalsChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            digimon.basicInfo.currentHP = event.currentHP;
            digimon.basicInfo.maxHP = event.maxHP;
            digimon.basicInfo.currentMP = event.currentMP;
            digimon.basicInfo.maxMP = event.maxMP;
        }
    }

    public static updateExperience(state: State | null, event: Events.DigimonExperienceChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            digimon.basicInfo.level = event.level;
            digimon.basicInfo.experience = event.currentEXP;
        }
    }

    public static updateLevel(state: State | null, event: Events.DigimonLevelChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            digimon.basicInfo.level = event.newLevel;
        }
    }

    public static updateAttributes(state: State | null, event: Events.DigimonAttributesChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            const attrs = digimon.attributes;
            attrs.strength = event.strength;
            attrs.defense = event.defense;
            attrs.spirit = event.spirit;
            attrs.wisdom = event.wisdom;
            attrs.speed = event.speed;
            attrs.charisma = event.charisma;
        }
    }

    public static updateResistances(state: State | null, event: Events.DigimonResistancesChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            const res = digimon.resistances;
            res.fire = event.fire;
            res.water = event.water;
            res.ice = event.ice;
            res.wind = event.wind;
            res.thunder = event.thunder;
            res.machine = event.machine;
            res.dark = event.dark;
        }
    }

    public static updateEquipments(state: State | null, event: Events.DigimonEquipmentsChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            digimon.equipments = { ...event.equipments };
        }
    }

    public static updateDigievolutions(state: State | null, event: Events.DigimonDigievolutionsChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            digimon.equippedDigievolutions = event.equippedDigievolutions.map(evoDto => {
                if (!evoDto) return null;
                return {
                    id: evoDto.id,
                    level: evoDto.level,
                    name: GameConverter.toDigievolutionName(evoDto.id)
                };
            });
        }
    }

    public static updateActiveDigievolution(state: State | null, event: Events.DigimonActiveDigievolutionChangedDTO) {
        const digimon = state?.party?.slots[event.partySlotIndex];
        if (digimon) {
            digimon.activeDigievolutionId = event.activeDigievolutionId;
        }
    }
}
