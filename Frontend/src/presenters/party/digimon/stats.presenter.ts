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
import { EquipmentsHelper } from "@/presenters/helper/equipments.helper";
import { ProfilePresenter } from "@/presenters/party/digimon/profile.presenter";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";
import { EquipmentRepository } from "@/repositories/equipment.repository";
import { EquipmentService } from "@/services/equipment.service";
import type { EquipmentRaw } from "@/repositories/tables/raws/equipment/equipment.raw";
import type { AttributesViewModel } from "@/viewmodels/digimon/attributes.viewmodel";
import type { DigimonStatsViewModel } from "@/viewmodels/digimon/digimon-stats.viewmodel";
import type { DigievolutionViewModel } from "@/viewmodels/digievolution/digievolution.viewmodel";

export class StatsPresenter {
  public static getStatsViewModel(digimon: Digimon, location: string | null): DigimonStatsViewModel {
    const isInBattle = ProfilePresenter.isInBattle(location, digimon.inBattle);
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
      strength: EquipmentsHelper.calculateBonus(Constant.strength, rawEquipments),
      defense: EquipmentsHelper.calculateBonus(Constant.defense, rawEquipments),
      spirit: EquipmentsHelper.calculateBonus(Constant.spirit, rawEquipments),
      wisdom: EquipmentsHelper.calculateBonus(Constant.wisdom, rawEquipments),
      speed: EquipmentsHelper.calculateBonus(Constant.speed, rawEquipments),
      charisma: EquipmentsHelper.calculateBonus(Constant.charisma, rawEquipments),
    };
  }

  private static getResistancesEquipmentBonuses(
    rawEquipments: EquipmentRaw[],
  ): ResistancesEquipmentBonuses {
    return {
      fire: EquipmentsHelper.calculateBonus(Constant.fire, rawEquipments),
      water: EquipmentsHelper.calculateBonus(Constant.water, rawEquipments),
      ice: EquipmentsHelper.calculateBonus(Constant.ice, rawEquipments),
      wind: EquipmentsHelper.calculateBonus(Constant.wind, rawEquipments),
      thunder: EquipmentsHelper.calculateBonus(Constant.thunder, rawEquipments),
      machine: EquipmentsHelper.calculateBonus(Constant.machine, rawEquipments),
      dark: EquipmentsHelper.calculateBonus(Constant.dark, rawEquipments),
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
