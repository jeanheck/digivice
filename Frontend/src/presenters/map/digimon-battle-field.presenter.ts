import type { ComposerTranslation } from "vue-i18n";
import { resolveBattleFieldElement, resolveFieldTechniqueKey } from "@/constants/battle-field.constant";
import { FieldRepository } from "@/repositories/field.repository";
import type { DigimonBattleFieldViewModel } from "@/viewmodels/map/digimon-battle-field.viewmodel";

export class DigimonBattleFieldPresenter {
  public static getDigimonBattleFieldViewModel(
    fieldId: number,
    translate: ComposerTranslation,
  ): DigimonBattleFieldViewModel {
    if (fieldId === 0) {
      return this.getNeutralDigimonBattleField(translate);
    }

    const fieldRaw = FieldRepository.getByFieldId(fieldId);
    if (fieldRaw === null) {
      return this.getNeutralDigimonBattleField(translate);
    }

    const element = resolveBattleFieldElement(fieldId);
    if (element === null) {
      return this.getNeutralDigimonBattleField(translate);
    }

    const techniqueKey = resolveFieldTechniqueKey(element);

    return {
      title: translate(`technique.${techniqueKey}.name`),
      strengthen: translate("map.fieldStrengthen", {
        element: translate(`stat.${fieldRaw.strengthens}`),
      }),
      weaken: translate("map.fieldWeaken", {
        element: translate(`stat.${fieldRaw.weakens}`),
      }),
    };
  }

  private static getNeutralDigimonBattleField(translate: ComposerTranslation): DigimonBattleFieldViewModel {
    return {
      title: translate("map.fieldNeutral"),
      strengthen: null,
      weaken: null,
    };
  }
}
