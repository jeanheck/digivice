import type { DigievolutionTreeFamilyNodeViewModel } from "./digievolution-tree-family-node.viewmodel";

export interface DigievolutionTreeFamilyViewModel {
    key: string;
    nodesBeforeFork: DigievolutionTreeFamilyNodeViewModel[];
    branchs: DigievolutionTreeFamilyNodeViewModel[][];
}
