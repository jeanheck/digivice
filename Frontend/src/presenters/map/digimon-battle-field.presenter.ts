import { resolveBattleFieldElement, resolveFieldTechniqueKey } from "@/constants/battle-field.constant";
import { FieldRepository } from "@/repositories/field.repository";
import type { DigimonBattleFieldViewModel } from "@/viewmodels/map/digimon-battle-field.viewmodel";

export class DigimonBattleFieldPresenter {
  public static getDigimonBattleFieldViewModel(fieldId: number): DigimonBattleFieldViewModel {
    if (fieldId === 0) {
      return this.getNeutralDigimonBattleField();
    }

    const fieldRaw = FieldRepository.getByFieldId(fieldId);
    if (fieldRaw === null) {
      return this.getNeutralDigimonBattleField();
    }

    const element = resolveBattleFieldElement(fieldId);
    if (element === null) {
      return this.getNeutralDigimonBattleField();
    }

    const techniqueKey = resolveFieldTechniqueKey(element);

    return {
      type: `technique.${techniqueKey}.name`,
      strengthen: fieldRaw.strengthens,
      weaken: fieldRaw.weakens,
    };
  }

  private static getNeutralDigimonBattleField(): DigimonBattleFieldViewModel {
    return {
      type: "map.fieldNeutral",
      strengthen: null,
      weaken: null,
    };
  }
}
