import { DigimonStatusCalculator } from "@/logic/DigimonStatusCalculator";
import { type Digimon, type DigimonSlot } from "@/models";
import { Stat } from "@/models/stat";
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
            const rawEquipments = EquipmentRepository.getRawEquipmentsByIds(digimon.equipments);
            return DigimonStatusCalculator.calculateBonusFromEquipaments(
                Stat.charisma,
                rawEquipments
            );
        }));

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}
