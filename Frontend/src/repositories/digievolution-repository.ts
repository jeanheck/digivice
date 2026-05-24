import DigievolutionJson from '@/database/digievolution/digievolution.json';
import DigievolutionTechniquesJson from '@/database/digievolution/digievolution-technique.json';
import type { DigievolutionTable } from '@/repositories/tables/digievolution/digievolution-table';
import type { DigievolutionTechniqueTable } from '@/repositories/tables/digievolution/digievolution-technique-table';
import type { DigievolutionRaw } from './tables/raws/digievolution/digievolution-raw';
import type { DigievolutionTechniqueRaw } from './tables/raws/digievolution/digievolution-technique-raw';

export class DigievolutionRepository {
    private static readonly digievolutionTable = DigievolutionJson as DigievolutionTable;
    private static readonly digievolutionTechniqueTable = DigievolutionTechniquesJson as DigievolutionTechniqueTable;

    public static getDigievolutionNameById(id: number): string {
        return this.digievolutionTable[id]!.name;
    }
    public static getRawDigievolution(id: number): DigievolutionRaw {
        return this.digievolutionTable[id]!;
    }
    public static getRawDigievolutionTechniquesById(id: number): DigievolutionTechniqueRaw[] {
        return this.digievolutionTechniqueTable[id]!;
    }
}
