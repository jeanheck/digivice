import DigievolutionTreeJson from '@/database/digievolution/digievolution-tree.json';
import type { DigievolutionTreeTable } from './tables/digievolution/digievolution-tree-table';
import type { DigievolutionTreeRaw } from './tables/raws/digievolution/digievolution-tree-raw';

export class DigievolutionTreeRepository {
    private static readonly digievolutionTreeTable = DigievolutionTreeJson as DigievolutionTreeTable;

    public static getDigievolutionTreeByFamily(family: string): DigievolutionTreeRaw[] {
        return this.digievolutionTreeTable[family]!;
    }
    public static getDigievolutionTree(): DigievolutionTreeTable {
        return this.digievolutionTreeTable;
    }
}
