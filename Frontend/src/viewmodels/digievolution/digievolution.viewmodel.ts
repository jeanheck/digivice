import type { DigievolutionAttributesViewModel } from "./digievolution-attributes.viewmodel";
import type { DigievolutionResistancesViewModel } from "./digievolution-resistances.viewmodel";

export interface DigievolutionViewModel {
    name: string;
    attributes: DigievolutionAttributesViewModel;
    resistances: DigievolutionResistancesViewModel;
}