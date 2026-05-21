import { MathUtils } from '@/utils/MathUtils';
import type { DigimonSlot } from '../models';
import { DigimonStatusType } from '../models';
import { DigimonStatusCalculator } from './DigimonStatusCalculator';
import { Repository } from '../repositories/repository';
import { PartyHelper } from './helpers/PartyHelper';

export class PartyCalculator {
    public static calculatePartyCharisma(slots: DigimonSlot[]): number {
        const digimons = PartyHelper.getDigimons(slots);
        const totalDigimonsCharisma = MathUtils.Sum(digimons.map((d) => Number(d.attributes.charisma)));

        let totalBonusFromEquipments = 0;

        for (const digimon of digimons) {
            const activeEquips = Repository.getEquipmentsByIds(digimon.equipments);
            const bonus = DigimonStatusCalculator.calculateBonusFromEquipaments(
                DigimonStatusType.charisma,
                activeEquips
            );
            totalBonusFromEquipments += bonus;
        }

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}

