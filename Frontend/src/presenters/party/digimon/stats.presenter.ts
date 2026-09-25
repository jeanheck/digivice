import type { Digimon } from "@/models";
import { Constant } from "@/constants/constant";
import {
  AttributesConverter,
  type AttributesEquipmentBonuses,
} from "@/presenters/converter/attributes.converter";
import {
  ResistancesConverter,
  type ResistancesEquipmentBonuses,
} from "@/presenters/converter/resistances.converter";
import { DigimonBattleService } from "@/services/digimon-battle.service";
import { EquipmentService } from "@/services/equipment.service";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { AttributesViewModel } from "@/viewmodels/digimon/attributes.viewmodel";
import type { DigimonStatsViewModel } from "@/viewmodels/digimon/digimon-stats.viewmodel";
import type { DigievolutionViewModel } from "@/viewmodels/digievolution/digievolution.viewmodel";

export class StatsPresenter {
  public static getStatsViewModel(digimon: Digimon, location: string | null): DigimonStatsViewModel {
    const isInBattle = DigimonBattleService.isInBattle(location, digimon.inBattle);
    const activeDigievolution =
      digimon.activeDigievolutionId !== null && digimon.activeDigievolutionId !== 0
        ? this.getDigievolutionById(digimon.activeDigievolutionId)
        : null;
    const equipmentIds = EquipmentService.getEquipmentIds(digimon.equipments);
    const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);
    const attributes = AttributesConverter.convert(
      digimon.attributes,
      activeDigievolution?.attributes ?? null,
      this.getAttributesEquipmentBonuses(rawEquipments),
    );

    return {
      attributes: this.applyBattleDeltas(attributes, digimon.inBattle, isInBattle),
      resistances: ResistancesConverter.convert(
        digimon.resistances,
        activeDigievolution?.resistances ?? null,
        this.getResistancesEquipmentBonuses(rawEquipments),
      ),
    };
  }

  private static applyBattleDeltas(
    attributes: AttributesViewModel,
    inBattle: Digimon["inBattle"],
    isInBattle: boolean,
  ): AttributesViewModel {
    return {
      ...attributes,
      strength: { ...attributes.strength, fromBattle: isInBattle ? inBattle.strength : 0 },
      defense: { ...attributes.defense, fromBattle: isInBattle ? inBattle.defense : 0 },
      speed: { ...attributes.speed, fromBattle: isInBattle ? inBattle.speed : 0 },
    };
  }

  private static getAttributesEquipmentBonuses(
    rawEquipments: EquipmentRaw[],
  ): AttributesEquipmentBonuses {
    return {
      strength: EquipmentService.calculateBonus(Constant.strength, rawEquipments),
      defense: EquipmentService.calculateBonus(Constant.defense, rawEquipments),
      spirit: EquipmentService.calculateBonus(Constant.spirit, rawEquipments),
      wisdom: EquipmentService.calculateBonus(Constant.wisdom, rawEquipments),
      speed: EquipmentService.calculateBonus(Constant.speed, rawEquipments),
      charisma: EquipmentService.calculateBonus(Constant.charisma, rawEquipments),
    };
  }

  private static getResistancesEquipmentBonuses(
    rawEquipments: EquipmentRaw[],
  ): ResistancesEquipmentBonuses {
    return {
      fire: EquipmentService.calculateBonus(Constant.fire, rawEquipments),
      water: EquipmentService.calculateBonus(Constant.water, rawEquipments),
      ice: EquipmentService.calculateBonus(Constant.ice, rawEquipments),
      wind: EquipmentService.calculateBonus(Constant.wind, rawEquipments),
      thunder: EquipmentService.calculateBonus(Constant.thunder, rawEquipments),
      machine: EquipmentService.calculateBonus(Constant.machine, rawEquipments),
      dark: EquipmentService.calculateBonus(Constant.dark, rawEquipments),
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
