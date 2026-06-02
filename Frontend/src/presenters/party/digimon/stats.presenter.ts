import type { Digimon } from "@/models";
import { Constant } from "@/constants/constant";
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
            strength: EquipmentsHelper.calculateBonusFromEquipaments(Constant.strength, rawEquipments),
            defense: EquipmentsHelper.calculateBonusFromEquipaments(Constant.defense, rawEquipments),
            spirit: EquipmentsHelper.calculateBonusFromEquipaments(Constant.spirit, rawEquipments),
            wisdom: EquipmentsHelper.calculateBonusFromEquipaments(Constant.wisdom, rawEquipments),
            speed: EquipmentsHelper.calculateBonusFromEquipaments(Constant.speed, rawEquipments),
            charisma: EquipmentsHelper.calculateBonusFromEquipaments(Constant.charisma, rawEquipments),
        };
    }

    private static getResistancesEquipmentBonuses(rawEquipments: EquipmentRaw[]): ResistancesEquipmentBonuses {
        return {
            fire: EquipmentsHelper.calculateBonusFromEquipaments(Constant.fire, rawEquipments),
            water: EquipmentsHelper.calculateBonusFromEquipaments(Constant.water, rawEquipments),
            ice: EquipmentsHelper.calculateBonusFromEquipaments(Constant.ice, rawEquipments),
            wind: EquipmentsHelper.calculateBonusFromEquipaments(Constant.wind, rawEquipments),
            thunder: EquipmentsHelper.calculateBonusFromEquipaments(Constant.thunder, rawEquipments),
            machine: EquipmentsHelper.calculateBonusFromEquipaments(Constant.machine, rawEquipments),
            dark: EquipmentsHelper.calculateBonusFromEquipaments(Constant.dark, rawEquipments),
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
