import type { TechniqueTable } from '@/repositories/tables/technique-table';
import TechniqueJson from '@/database/digievolution/technique.json';
import type { TechniqueRaw } from './tables/raws/digievolution/technique-raw';

export class TechniqueRepository {
    private static readonly techniqueTable = TechniqueJson as TechniqueTable;

    public static getTechniqueById(id: string): TechniqueRaw {
        return this.techniqueTable[id]!;
    }
}
