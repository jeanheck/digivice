import type { Digimon } from "@/models";
import type { Equipments } from "@/models/equipments";
import { Stat } from "@/models/stat";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import { MathUtils } from "@/utils/MathUtils";
import type { AttributesViewModel } from "@/viewmodels/digimon/attributes.viewmodel";
import type { DigimonStatsViewModel } from "@/viewmodels/digimon/digimon-stats.viewmodel";
import type { ResistancesViewModel } from "@/viewmodels/digimon/resistances.viewmodel";
import type { StatViewModel } from "@/viewmodels/digimon/stat.viewmodel";
import type { DigievolutionViewModel } from "@/viewmodels/digievolution/digievolution.viewmodel";

export class DigimonStatsPresenter {
    public static getStatsViewModel(digimon: Digimon): DigimonStatsViewModel {
        const activeDigievolution = digimon.activeDigievolutionId !== null && digimon.activeDigievolutionId !== 0
            ? DigimonStatsPresenter.getDigievolutionById(digimon.activeDigievolutionId)
            : null;

        return {
            attributes: DigimonStatsPresenter.createAttributesViewModel(digimon, activeDigievolution),
            resistances: DigimonStatsPresenter.createResistancesViewModel(digimon, activeDigievolution),
        };
    }

    private static getDigievolutionById(digievolutionId: number): DigievolutionViewModel {
        const digievolutionRaw = DigievolutionRepository.getRawDigievolutionById(digievolutionId);

        return {
            name: digievolutionRaw.name,
            attributes: digievolutionRaw.attributes,
            resistances: digievolutionRaw.resistances,
        };
    }

    private static createAttributesViewModel(
        digimon: Digimon,
        activeDigievolution: DigievolutionViewModel | null
    ): AttributesViewModel {
        return {
            strength: DigimonStatsPresenter.createStatViewModel(
                Stat.strength,
                digimon.attributes.strength,
                activeDigievolution?.attributes.strength ?? 0,
                digimon.equipments
            ),
            defense: DigimonStatsPresenter.createStatViewModel(
                Stat.defense,
                digimon.attributes.defense,
                activeDigievolution?.attributes.defense ?? 0,
                digimon.equipments
            ),
            spirit: DigimonStatsPresenter.createStatViewModel(
                Stat.spirit,
                digimon.attributes.spirit,
                activeDigievolution?.attributes.spirit ?? 0,
                digimon.equipments
            ),
            wisdom: DigimonStatsPresenter.createStatViewModel(
                Stat.wisdom,
                digimon.attributes.wisdom,
                activeDigievolution?.attributes.wisdom ?? 0,
                digimon.equipments
            ),
            speed: DigimonStatsPresenter.createStatViewModel(
                Stat.speed,
                digimon.attributes.speed,
                activeDigievolution?.attributes.speed ?? 0,
                digimon.equipments
            ),
            charisma: DigimonStatsPresenter.createStatViewModel(
                Stat.charisma,
                digimon.attributes.charisma,
                activeDigievolution?.attributes.charisma ?? 0,
                digimon.equipments
            ),
        };
    }

    private static createResistancesViewModel(
        digimon: Digimon,
        activeDigievolution: DigievolutionViewModel | null
    ): ResistancesViewModel {
        return {
            fire: DigimonStatsPresenter.createStatViewModel(
                Stat.fire,
                digimon.resistances.fire,
                activeDigievolution?.resistances.fire ?? 0,
                digimon.equipments
            ),
            water: DigimonStatsPresenter.createStatViewModel(
                Stat.water,
                digimon.resistances.water,
                activeDigievolution?.resistances.water ?? 0,
                digimon.equipments
            ),
            ice: DigimonStatsPresenter.createStatViewModel(
                Stat.ice,
                digimon.resistances.ice,
                activeDigievolution?.resistances.ice ?? 0,
                digimon.equipments
            ),
            wind: DigimonStatsPresenter.createStatViewModel(
                Stat.wind,
                digimon.resistances.wind,
                activeDigievolution?.resistances.wind ?? 0,
                digimon.equipments
            ),
            thunder: DigimonStatsPresenter.createStatViewModel(
                Stat.thunder,
                digimon.resistances.thunder,
                activeDigievolution?.resistances.thunder ?? 0,
                digimon.equipments
            ),
            machine: DigimonStatsPresenter.createStatViewModel(
                Stat.machine,
                digimon.resistances.machine,
                activeDigievolution?.resistances.machine ?? 0,
                digimon.equipments
            ),
            dark: DigimonStatsPresenter.createStatViewModel(
                Stat.dark,
                digimon.resistances.dark,
                activeDigievolution?.resistances.dark ?? 0,
                digimon.equipments
            ),
        };
    }

    private static calculateBonusFromEquipaments(type: Stat, rawEquipments: EquipmentRaw[]): number {
        const lowerCaseType = type.toLowerCase();
        const attributesRaw = rawEquipments
            .flatMap(rawEquipment => rawEquipment.attributes)
            .filter(attribute => attribute.attribute.toLowerCase() === lowerCaseType);

        return MathUtils.Sum(attributesRaw.map(attribute => {
            return Number(`${attribute.type}${attribute.value}`);
        }));
    }

    private static calculateBonusFromRawEquipments(type: Stat, equipments: Equipments): number {
        const rawEquipments = EquipmentRepository.getRawEquipmentsByIds(equipments);
        return DigimonStatsPresenter.calculateBonusFromEquipaments(type, rawEquipments);
    }

    private static createStatViewModel(
        stat: Stat,
        fromDigimon: number,
        fromDigievolution: number,
        equipments: Equipments
    ): StatViewModel {
        const fromEquipaments = DigimonStatsPresenter.calculateBonusFromRawEquipments(stat, equipments);

        return {
            fromDigimon,
            fromEquipaments,
            fromDigievolution,
            sumBetweenDigimonAndEquipaments: fromDigimon + fromEquipaments,
        };
    }
}
