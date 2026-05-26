import DigimonDigievolutionJson from '@/database/digimon/digimon-digievolution.json';
import type { DigimonDigievolutionTable } from './tables/digimon/digimon-digievolution-table';
import type { DigimonDigievolutionRaw } from './tables/raws/digimon/digimon-digievolution-raw';
import type { DigimonDigievolutionRequirementRaw } from './tables/raws/digimon/digimon-digievolution-requirement-raw';

export class DigimonDigievolutionRepository {
    private static readonly digimonDigievolutionTable = DigimonDigievolutionJson as DigimonDigievolutionTable;

    public static getDigievolutionsByDigimonName(digimonName: string): DigimonDigievolutionRaw {
        return this.digimonDigievolutionTable[digimonName]!;
    }
    public static getDigievolutionRequirements(digimonName: string, digievolutionName: string): DigimonDigievolutionRequirementRaw[] {
        return this.digimonDigievolutionTable[digimonName]![digievolutionName]!;
    }
}
