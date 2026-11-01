import type { Digimon } from "@/state/models";
import {
  AttributesConverter,
  type AttributesEquipmentBonuses,
  type AttributesEquipmentSetBonuses,
} from "@/presentation/presenters/converter/attributes.converter";
import {
  ElementsConverter,
  type ElementsEquipmentBonuses,
} from "@/presentation/presenters/converter/elements.converter";
import { EquipmentService } from "@/presentation/services/equipment.service";
import { DigimonBattleSelector } from "@/state/stores/selectors/digimon-battle.selector";
import { DigievolutionRepository } from "@/presentation/repositories/digievolution.repository";
import { EquipmentRepository } from "@/presentation/repositories/equipment.repository";
import type { EquipmentRaw } from "@/presentation/repositories/tables/raws/equipment/equipment.raw";
import type { EquipmentSetRaw } from "@/presentation/repositories/tables/raws/equipment/equipment-set.raw";
import type { AttributesViewModel } from "@/presentation/viewmodels/digimon/attributes.viewmodel";
import type { DigimonStatsViewModel } from "@/presentation/viewmodels/digimon/digimon-stats.viewmodel";
import type { DigievolutionViewModel } from "@/presentation/viewmodels/digievolution/digievolution.viewmodel";

export class StatsPresenter {
  public static getStatsViewModel(
    digimon: Digimon,
    digimonId: number,
    location: string,
  ): DigimonStatsViewModel {
    const isInBattle = DigimonBattleSelector.isInBattle(location, digimon.inBattle);
    const activeDigievolution =
      digimon.activeDigievolutionId !== null
        ? this.getDigievolutionById(digimon.activeDigievolutionId)
        : null;
    const equipmentIds = EquipmentService.getEquipmentIds(digimon.equipments);
    const rawEquipments = EquipmentRepository.getEquipmentsByIds(equipmentIds);
    const equipmentSet = EquipmentService.getCompletedSet(digimonId, equipmentIds);
    const attributes = AttributesConverter.convert(
      digimon.attributes,
      activeDigievolution?.attributes ?? null,
      this.getAttributesEquipmentBonuses(rawEquipments),
      this.getAttributesEquipmentSetBonuses(equipmentSet),
    );

    return {
      attributes: this.applyBattleDeltas(attributes, digimon.inBattle, isInBattle),
      elements: ElementsConverter.convert(
        digimon.elements,
        activeDigievolution?.elements ?? null,
        this.getElementsEquipmentBonuses(rawEquipments),
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
      strength: EquipmentService.calculateBonus("strength", rawEquipments),
      defense: EquipmentService.calculateBonus("defense", rawEquipments),
      spirit: EquipmentService.calculateBonus("spirit", rawEquipments),
      wisdom: EquipmentService.calculateBonus("wisdom", rawEquipments),
      speed: EquipmentService.calculateBonus("speed", rawEquipments),
      charisma: EquipmentService.calculateBonus("charisma", rawEquipments),
    };
  }

  private static getAttributesEquipmentSetBonuses(
    equipmentSet: EquipmentSetRaw | null,
  ): AttributesEquipmentSetBonuses {
    return {
      strength: EquipmentService.calculateSetBonus("strength", equipmentSet),
      defense: EquipmentService.calculateSetBonus("defense", equipmentSet),
      spirit: EquipmentService.calculateSetBonus("spirit", equipmentSet),
      wisdom: EquipmentService.calculateSetBonus("wisdom", equipmentSet),
      speed: EquipmentService.calculateSetBonus("speed", equipmentSet),
      charisma: EquipmentService.calculateSetBonus("charisma", equipmentSet),
    };
  }

  private static getElementsEquipmentBonuses(
    rawEquipments: EquipmentRaw[],
  ): ElementsEquipmentBonuses {
    return {
      fire: EquipmentService.calculateBonus("fire", rawEquipments),
      water: EquipmentService.calculateBonus("water", rawEquipments),
      ice: EquipmentService.calculateBonus("ice", rawEquipments),
      wind: EquipmentService.calculateBonus("wind", rawEquipments),
      thunder: EquipmentService.calculateBonus("thunder", rawEquipments),
      machine: EquipmentService.calculateBonus("machine", rawEquipments),
      dark: EquipmentService.calculateBonus("dark", rawEquipments),
    };
  }

  private static getDigievolutionById(digievolutionId: number): DigievolutionViewModel {
    const digievolutionRaw = DigievolutionRepository.getRawDigievolutionById(digievolutionId);

    return {
      name: digievolutionRaw.name,
      attributes: digievolutionRaw.attributes,
      elements: digievolutionRaw.elements,
      resistances: digievolutionRaw.resistances,
    };
  }
}
