import { type Digimon, type DigimonSlot } from "@/models";
import { StatKey } from "@/constants/stat/stat-key";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import { MathUtils } from "@/utils/MathUtils";

export class FooterPresenter {
    private static getDigimons(slots: DigimonSlot[]): Digimon[] {
        return slots
            .map(slot => slot.digimon)
            .filter(digimon => digimon !== null);
    }

    public static getPartyCharisma(digimonSlots: DigimonSlot[]): number {
        const digimons = this.getDigimons(digimonSlots);
        const totalDigimonsCharisma = MathUtils.Sum(digimons.map((d) => Number(d.attributes.charisma)));
        const totalBonusFromEquipments = MathUtils.Sum(digimons.map((digimon) => {
            const equipmentIds = EquipmentsHelper.getUniqueEquipmentIds(digimon.equipments);
            const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);
            return EquipmentsHelper.calculateBonusFromEquipaments(
                StatKey.charisma,
                rawEquipments
            );
        }));

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}
