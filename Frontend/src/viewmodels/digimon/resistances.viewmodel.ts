import type { AttributeResistanceViewModel } from "./attribute-resistance.viewmodel";

export interface ResistancesViewModel {
  fire: AttributeResistanceViewModel;
  water: AttributeResistanceViewModel;
  ice: AttributeResistanceViewModel;
  wind: AttributeResistanceViewModel;
  thunder: AttributeResistanceViewModel;
  machine: AttributeResistanceViewModel;
  dark: AttributeResistanceViewModel;
}
