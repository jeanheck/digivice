import type { Digimon } from "@/models";
import { AttributesConverter } from "@/presenters/converter/attributes.converter";
import { ResistancesConverter } from "@/presenters/converter/resistances.converter";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
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
                rawEquipments
            ),
            resistances: ResistancesConverter.convert(
                digimon.resistances,
                activeDigievolution?.resistances ?? null,
                rawEquipments
            ),
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
