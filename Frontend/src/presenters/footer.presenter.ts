import { type Digimon, type DigimonSlot } from "@/models";
import { Stat } from "@/models/stat";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import { MathUtils } from "@/utils/MathUtils";

export class FooterPresenter {
    private static getDigimons(slots: DigimonSlot[]): Digimon[] {
        return slots
            .map(slot => slot.digimon)
            .filter(digimon => digimon !== null);
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

    public static getPartyCharisma(digimonSlots: DigimonSlot[]): number {
        const digimons = this.getDigimons(digimonSlots);
        const totalDigimonsCharisma = MathUtils.Sum(digimons.map((d) => Number(d.attributes.charisma)));
        const totalBonusFromEquipments = MathUtils.Sum(digimons.map((digimon) => {
            const equipmentIds = EquipmentsHelper.getUniqueEquipmentIds(digimon.equipments);
            const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);
            return this.calculateBonusFromEquipaments(
                Stat.charisma,
                rawEquipments
            );
        }));

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}
