import type { DigievolutionTreeNodeViewModel } from "./digievolution-tree-node.viewmodel";

export interface DigievolutionTreeFamilyViewModel {
    familyKey: string;
    nodesBeforeFork: DigievolutionTreeNodeViewModel[];
    branchs: DigievolutionTreeNodeViewModel[][];
}
