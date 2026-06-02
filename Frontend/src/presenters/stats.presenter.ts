import type { Digimon } from "@/models";
import { Constants } from "@/constants/constants";
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

export class StatsPresenter {
    public static getStatsViewModel(digimon: Digimon): DigimonStatsViewModel {
        const activeDigievolution = digimon.activeDigievolutionId !== null && digimon.activeDigievolutionId !== 0
            ? this.getDigievolutionById(digimon.activeDigievolutionId)
            : null;
        const equipmentIds = EquipmentsHelper.getUniqueEquipmentIds(digimon.equipments);
        const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);

        return {
            attributes: AttributesConverter.convert(
                digimon.attributes,
                activeDigievolution?.attributes ?? null,
                this.getAttributesEquipmentBonuses(rawEquipments)
            ),
            resistances: ResistancesConverter.convert(
                digimon.resistances,
                activeDigievolution?.resistances ?? null,
                this.getResistancesEquipmentBonuses(rawEquipments)
            ),
        };
    }

    private static getAttributesEquipmentBonuses(rawEquipments: EquipmentRaw[]): AttributesEquipmentBonuses {
        return {
            strength: EquipmentsHelper.calculateBonusFromEquipaments(Constants.strength, rawEquipments),
            defense: EquipmentsHelper.calculateBonusFromEquipaments(Constants.defense, rawEquipments),
            spirit: EquipmentsHelper.calculateBonusFromEquipaments(Constants.spirit, rawEquipments),
            wisdom: EquipmentsHelper.calculateBonusFromEquipaments(Constants.wisdom, rawEquipments),
            speed: EquipmentsHelper.calculateBonusFromEquipaments(Constants.speed, rawEquipments),
            charisma: EquipmentsHelper.calculateBonusFromEquipaments(Constants.charisma, rawEquipments),
        };
    }

    private static getResistancesEquipmentBonuses(rawEquipments: EquipmentRaw[]): ResistancesEquipmentBonuses {
        return {
            fire: EquipmentsHelper.calculateBonusFromEquipaments(Constants.fire, rawEquipments),
            water: EquipmentsHelper.calculateBonusFromEquipaments(Constants.water, rawEquipments),
            ice: EquipmentsHelper.calculateBonusFromEquipaments(Constants.ice, rawEquipments),
            wind: EquipmentsHelper.calculateBonusFromEquipaments(Constants.wind, rawEquipments),
            thunder: EquipmentsHelper.calculateBonusFromEquipaments(Constants.thunder, rawEquipments),
            machine: EquipmentsHelper.calculateBonusFromEquipaments(Constants.machine, rawEquipments),
            dark: EquipmentsHelper.calculateBonusFromEquipaments(Constants.dark, rawEquipments),
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
