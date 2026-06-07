import { type Digimon, type DigimonSlot } from "@/models";
import { Constant } from "@/constants/constant";
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import { MathHelper } from "@/presenters/helper/math.helper";

export class FooterPresenter {
    private static getDigimons(slots: DigimonSlot[]): Digimon[] {
        return slots
            .map(slot => slot.digimon)
            .filter(digimon => digimon !== null);
    }

    public static getPartyCharisma(digimonSlots: DigimonSlot[]): number {
        const digimons = this.getDigimons(digimonSlots);
        const totalDigimonsCharisma = MathHelper.sum(digimons.map((d) => Number(d.attributes.charisma)));
        const totalBonusFromEquipments = MathHelper.sum(digimons.map((digimon) => {
            const equipmentIds = EquipmentsHelper.getBonusCalculationEquipmentIds(
                digimon.equipments,
                (equipmentId) => EquipmentRepository.getEquipmentById(equipmentId).type
            );
            const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);
            return EquipmentsHelper.calculateBonusFromEquipaments(
                Constant.charisma,
                rawEquipments
            );
        }));

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}
