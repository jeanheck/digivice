import type { EnrichedDigievolution } from '@/models';
import DigievolutionJson from '@/database/digievolution/digievolution.json';
import DigievolutionTechniquesJson from '@/database/digievolution/digievolution-technique.json';
import type { DigievolutionTable } from '@/repositories/tables/digievolution-table';
import type { DigievolutionTechniqueTable } from '@/repositories/tables/digievolution-technique-table';

export class DigievolutionRepository {
    private static readonly digievolutionTable = DigievolutionJson as DigievolutionTable;
    private static readonly digievolutionTechniqueTable = DigievolutionTechniquesJson as DigievolutionTechniqueTable;

    public static getDigievolutionNameById(id: number): string {
        return this.digievolutionTable[id]!.name;
    }

    public static getEnrichedDigievolution(id: number): EnrichedDigievolution {
        return this.digievolutionTable[id]!;
    }
}
