import { MathUtils } from '@/utils/MathUtils';
import type { DigimonSlot } from '../models';
import { DigimonStatusType } from '../models';
import { DigimonStatusCalculator } from './DigimonStatusCalculator';
import { EquipmentsHelper } from './helpers/EquipmentsHelper';

export class PartyCalculator {
    public static calculatePartyCharisma(slots: DigimonSlot[]): number {
        const totalDigimonsCharisma = MathUtils.Sum(slots.map(s => Number(s.digimon?.attributes.charisma)));

        let totalBonusFromEquipments = 0;

        slots.forEach((slot) => {
            if (slot && slot.digimon) {
                const digimon = slot.digimon;
                const equipIds = [
                    digimon.equipments.head,
                    digimon.equipments.body,
                    digimon.equipments.rightHand,
                    digimon.equipments.leftHand,
                    digimon.equipments.accessory1,
                    digimon.equipments.accessory2
                ];

                const activeEquips = equipIds
                    .map((id) => {
                        return EquipmentsHelper.resolveEquipment(id);
                    })
                    .filter((e): e is any => {
                        return e !== null;
                    });

                const bonus = DigimonStatusCalculator.calculateBonusFromEquipaments(
                    DigimonStatusType.charisma,
                    activeEquips
                );
                totalBonusFromEquipments += bonus;
            }
        });

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}
