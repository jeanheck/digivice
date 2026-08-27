import type { ComposerTranslation } from "vue-i18n";
import { resolveBattleFieldElement, resolveFieldTechniqueKey } from "@/constants/battle-field.constant";
import { FieldRepository } from "@/repositories/field.repository";
import type { BattleFieldLabelsViewModel } from "@/viewmodels/map/battle-field-labels.viewmodel";

export class BattleFieldPresenter {
  public static getLabels(
    fieldId: number,
    translate: ComposerTranslation,
  ): BattleFieldLabelsViewModel {
    if (fieldId === 0) {
      return this.createNeutralLabels(translate);
    }

    const fieldRaw = FieldRepository.getByFieldId(fieldId);
    if (fieldRaw === null) {
      return this.createNeutralLabels(translate);
    }

    const element = resolveBattleFieldElement(fieldId);
    if (element === null) {
      return this.createNeutralLabels(translate);
    }

    const techniqueKey = resolveFieldTechniqueKey(element);

    return {
      title: translate(`technique.${techniqueKey}.name`),
      strengthenLabel: translate("map.fieldStrengthen", {
        element: translate(`stat.${fieldRaw.strengthens}`),
      }),
      weakenLabel: translate("map.fieldWeaken", {
        element: translate(`stat.${fieldRaw.weakens}`),
      }),
    };
  }

  private static createNeutralLabels(translate: ComposerTranslation): BattleFieldLabelsViewModel {
    return {
      title: translate("map.fieldNeutral"),
      strengthenLabel: null,
      weakenLabel: null,
    };
  }
}
