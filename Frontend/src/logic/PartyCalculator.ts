import { MathUtils } from '@/utils/MathUtils';
import type { DigimonSlot } from '../models';
import { DigimonStatusType } from '../models';
import { DigimonStatusCalculator } from './DigimonStatusCalculator';
import { Repository } from '../repositories/repository';
import { PartyHelper } from './helpers/PartyHelper';

export class PartyCalculator {
    public static calculatePartyCharisma(digimonSlots: DigimonSlot[]): number {
        const digimons = PartyHelper.getDigimons(digimonSlots);
        const totalDigimonsCharisma = MathUtils.Sum(digimons.map((d) => Number(d.attributes.charisma)));
        const totalBonusFromEquipments = MathUtils.Sum(digimons.map((digimon) => {
            const enrichedEquipments = Repository.getEquipmentsByIds(digimon.equipments);
            return DigimonStatusCalculator.calculateBonusFromEquipaments(
                DigimonStatusType.charisma,
                enrichedEquipments
            );
        }));

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}

