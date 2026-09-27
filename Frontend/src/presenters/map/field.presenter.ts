import { FieldRepository } from "@/repositories/field.repository";
import type { FieldViewModel } from "@/viewmodels/map/field.viewmodel";

export class FieldPresenter {
  public static getFieldViewModel(fieldId: number): FieldViewModel {
    const fieldRaw = FieldRepository.getFieldById(fieldId);
    if (fieldRaw === null) {
      return {
        name: "neutral",
        strengthen: null,
        weaken: null,
      };
    }

    return {
      name: fieldRaw.name,
      strengthen: fieldRaw.strengthens,
      weaken: fieldRaw.weakens,
    };
  }
}
