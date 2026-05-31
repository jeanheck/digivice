import type { Digimon } from "@/models";
import { Stat } from "@/models/stat";
import {
    AttributesConverter,
    type AttributesEquipmentBonuses,
} from "@/presenters/converter/attributes.converter";
import {
    ResistancesConverter,
    type ResistancesEquipmentBonuses,
} from "@/presenters/converter/resistances.converter";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { DigimonStatsViewModel } from "@/viewmodels/digimon/digimon-stats.viewmodel";
import type { DigievolutionViewModel } from "@/viewmodels/digievolution/digievolution.viewmodel";

export class DigimonStatsPresenter {
    public static getStatsViewModel(digimon: Digimon): DigimonStatsViewModel {
        const activeDigievolution = digimon.activeDigievolutionId !== null && digimon.activeDigievolutionId !== 0
            ? DigimonStatsPresenter.getDigievolutionById(digimon.activeDigievolutionId)
            : null;
        const equipmentIds = EquipmentsHelper.getUniqueEquipmentIds(digimon.equipments);
        const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);

        return {
            attributes: AttributesConverter.convert(
                digimon.attributes,
                activeDigievolution?.attributes ?? null,
                DigimonStatsPresenter.getAttributesEquipmentBonuses(rawEquipments)
            ),
            resistances: ResistancesConverter.convert(
                digimon.resistances,
                activeDigievolution?.resistances ?? null,
                DigimonStatsPresenter.getResistancesEquipmentBonuses(rawEquipments)
            ),
        };
    }

    private static getAttributesEquipmentBonuses(rawEquipments: EquipmentRaw[]): AttributesEquipmentBonuses {
        return {
            strength: EquipmentsHelper.calculateBonusFromEquipaments(Stat.strength, rawEquipments),
            defense: EquipmentsHelper.calculateBonusFromEquipaments(Stat.defense, rawEquipments),
            spirit: EquipmentsHelper.calculateBonusFromEquipaments(Stat.spirit, rawEquipments),
            wisdom: EquipmentsHelper.calculateBonusFromEquipaments(Stat.wisdom, rawEquipments),
            speed: EquipmentsHelper.calculateBonusFromEquipaments(Stat.speed, rawEquipments),
            charisma: EquipmentsHelper.calculateBonusFromEquipaments(Stat.charisma, rawEquipments),
        };
    }

    private static getResistancesEquipmentBonuses(rawEquipments: EquipmentRaw[]): ResistancesEquipmentBonuses {
        return {
            fire: EquipmentsHelper.calculateBonusFromEquipaments(Stat.fire, rawEquipments),
            water: EquipmentsHelper.calculateBonusFromEquipaments(Stat.water, rawEquipments),
            ice: EquipmentsHelper.calculateBonusFromEquipaments(Stat.ice, rawEquipments),
            wind: EquipmentsHelper.calculateBonusFromEquipaments(Stat.wind, rawEquipments),
            thunder: EquipmentsHelper.calculateBonusFromEquipaments(Stat.thunder, rawEquipments),
            machine: EquipmentsHelper.calculateBonusFromEquipaments(Stat.machine, rawEquipments),
            dark: EquipmentsHelper.calculateBonusFromEquipaments(Stat.dark, rawEquipments),
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
}
