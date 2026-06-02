import type { NodeViewModel } from "./node.viewmodel";

export interface DigievolutionTreeFamilyViewModel {
    key: string;
    nodesBeforeFork: NodeViewModel[];
    branchs: NodeViewModel[][];
}
