import type { DigievolutionAttributesViewModel } from "./digievolution-attributes.viewmodel";
import type { DigievolutionElementsViewModel } from "./digievolution-elements.viewmodel";
import type { DigievolutionResistancesViewModel } from "./digievolution-resistances.viewmodel";

export interface DigievolutionViewModel {
  name: string;
  attributes: DigievolutionAttributesViewModel;
  elements: DigievolutionElementsViewModel;
  resistances: DigievolutionResistancesViewModel;
}
