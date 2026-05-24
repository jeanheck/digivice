import type { DigimonExperienceTable } from "@/repositories/tables/digimon-experience-table";
import type { DigimonNameTable } from "@/repositories/tables/digimon-name-table";
import DigimonExperienceJson from "@/database/digimon/digimon-experience.json";
import DigimonNameJson from "@/database/digimon/digimon-name.json";

export class DigimonRepository {
    private static readonly digimonNameTable = DigimonNameJson as DigimonNameTable;
    private static readonly digimonExperienceTable = DigimonExperienceJson as DigimonExperienceTable;

    public static getRequiredExperienceForLevel(digimonName: string, level: number): number {
        return this.digimonExperienceTable[digimonName]![level]!;
    }
    public static getNameById(id: number): string {
        return this.digimonNameTable[id]!;
    }
}
