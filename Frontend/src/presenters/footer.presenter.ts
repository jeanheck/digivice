import { DigimonStatusCalculator } from "@/logic/DigimonStatusCalculator";
import { PartyHelper } from "@/logic/helpers/PartyHelper";
import { AttributeType, type DigimonSlot } from "@/models";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import { MathUtils } from "@/utils/MathUtils";

export class FooterPresenter {
    public static getPartyCharisma(digimonSlots: DigimonSlot[]): number {
        const digimons = PartyHelper.getDigimons(digimonSlots);
        const totalDigimonsCharisma = MathUtils.Sum(digimons.map((d) => Number(d.attributes.charisma)));
        const totalBonusFromEquipments = MathUtils.Sum(digimons.map((digimon) => {
            const rawEquipments = EquipmentRepository.getRawEquipmentsByIds(digimon.equipments);
            return DigimonStatusCalculator.calculateBonusFromEquipaments(
                AttributeType.charisma,
                rawEquipments
            );
        }));

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}
