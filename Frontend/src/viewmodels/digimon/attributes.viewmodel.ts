import type { AttributeResistanceViewModel } from "./attribute-resistance.viewmodel";

export interface AttributesViewModel {
  strength: AttributeResistanceViewModel;
  defense: AttributeResistanceViewModel;
  spirit: AttributeResistanceViewModel;
  wisdom: AttributeResistanceViewModel;
  speed: AttributeResistanceViewModel;
  charisma: AttributeResistanceViewModel;
}
