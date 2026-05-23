import type { TechniqueTable } from '@/repositories/tables/technique-table';
import TechniqueJson from '@/database/digievolution/technique.json';
import type { Technique } from '@/models';

export class TechniqueRepository {
    private static readonly TechniqueTable = TechniqueJson as TechniqueTable;

    public static getTechniqueById(id: string): Technique {
        return this.TechniqueTable[id]!;
    }
}
