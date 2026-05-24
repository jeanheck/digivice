import { MathUtils } from '@/utils/MathUtils';
import type { DigimonSlot } from '../models';
import { AttributeType } from '../models';
import { DigimonStatusCalculator } from './DigimonStatusCalculator';
import { EquipmentRepository } from '../repositories/equipment-repository';
import { PartyHelper } from './helpers/PartyHelper';

export class PartyCalculator {
    public static calculatePartyCharisma(digimonSlots: DigimonSlot[]): number {
        const digimons = PartyHelper.getDigimons(digimonSlots);
        const totalDigimonsCharisma = MathUtils.Sum(digimons.map((d) => Number(d.attributes.charisma)));
        const totalBonusFromEquipments = MathUtils.Sum(digimons.map((digimon) => {
            const enrichedEquipments = EquipmentRepository.getEnrichedEquipmentsByIds(digimon.equipments);            
            return DigimonStatusCalculator.calculateBonusFromEquipaments(
                AttributeType.Charisma,
                enrichedEquipments
            );
        }));

        

        return totalDigimonsCharisma + totalBonusFromEquipments;
    }
}

