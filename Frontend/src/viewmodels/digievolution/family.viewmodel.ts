import type { NodeViewModel } from "./node.viewmodel";

export interface FamilyViewModel {
    key: string;
    nodesBeforeFork: NodeViewModel[];
    branchs: NodeViewModel[][];
}
