import DigimonDigievolutionJson from "@/database/digimon/digimon-digievolution.json";
import DigimonExperienceJson from "@/database/digimon/digimon-experience.json";
import DigimonNameJson from "@/database/digimon/digimon-name.json";
import type { DigimonDigievolutionTable } from "@/repositories/tables/digimon/digimon-digievolution.table";
import type { DigimonExperienceTable } from "@/repositories/tables/digimon/digimon-experience.table";
import type { DigimonNameTable } from "@/repositories/tables/digimon/digimon-name.table";
import type { DigimonDigievolutionRaw } from "./tables/raws/digimon/digimon-digievolution.raw";
import type { DigimonDigievolutionRequirementRaw } from "./tables/raws/digimon/digimon-digievolution-requirement.raw";

export class DigimonRepository {
    private static readonly digimonNameTable = DigimonNameJson as DigimonNameTable;
    private static readonly digimonExperienceTable = DigimonExperienceJson as DigimonExperienceTable;
    private static readonly digimonDigievolutionTable = DigimonDigievolutionJson as DigimonDigievolutionTable;

    public static getRequiredExperienceForLevel(id: number, level: number): number {
        return this.digimonExperienceTable[id]![String(level)]!;
    }
    public static getNameById(id: number): string {
        return this.digimonNameTable[id]!;
    }
    public static getDigievolutionsById(id: number): DigimonDigievolutionRaw {
        return this.digimonDigievolutionTable[id]!;
    }
    public static getDigievolutionRequirements(digimonId: number, digievolutionId: number): DigimonDigievolutionRequirementRaw[] {
        return this.digimonDigievolutionTable[digimonId]![String(digievolutionId)]!;
    }
}
