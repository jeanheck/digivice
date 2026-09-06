import { DigimonBattleFieldRepository } from "@/repositories/digimon-battle-field.repository";
import type { DigimonBattleFieldViewModel } from "@/viewmodels/map/digimon-battle-field.viewmodel";

export class DigimonBattleFieldPresenter {
  public static getDigimonBattleFieldViewModel(fieldId: number): DigimonBattleFieldViewModel {
    if (fieldId === 0) {
      return this.getNeutralDigimonBattleField();
    }

    const fieldRaw = DigimonBattleFieldRepository.getByFieldId(fieldId);
    if (fieldRaw === null) {
      return this.getNeutralDigimonBattleField();
    }

    return {
      type: fieldRaw.type,
      strengthen: fieldRaw.strengthens,
      weaken: fieldRaw.weakens,
    };
  }

  private static getNeutralDigimonBattleField(): DigimonBattleFieldViewModel {
    return {
      type: "neutral",
      strengthen: null,
      weaken: null,
    };
  }
}
