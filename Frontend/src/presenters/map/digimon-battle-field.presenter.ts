import { DigimonBattleFieldRepository } from "@/repositories/digimon-battle-field.repository";
import type { DigimonBattleFieldViewModel } from "@/viewmodels/map/digimon-battle-field.viewmodel";

export class DigimonBattleFieldPresenter {
  public static getDigimonBattleFieldViewModel(fieldId: number): DigimonBattleFieldViewModel {
    const fieldRaw = DigimonBattleFieldRepository.getFieldById(fieldId);
    if (fieldRaw === null) {
      return {
        type: "neutral",
        strengthen: null,
        weaken: null,
      };
    }

    return {
      type: fieldRaw.type,
      strengthen: fieldRaw.strengthens,
      weaken: fieldRaw.weakens,
    };
  }
}
