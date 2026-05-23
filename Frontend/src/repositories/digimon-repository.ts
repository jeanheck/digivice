import type { DigimonExperienceTable } from "@/repositories/types/digimon-experience-table";
import type { DigimonNameTable } from "@/repositories/types/digimon-name-table";
import DigimonExperienceJson from "@/tables/digimon/digimon-experience.json";
import DigimonNameJson from "@/tables/digimon/digimon-name.json";

export class DigimonRepository {
    private static readonly DigimonNameTable = DigimonNameJson as DigimonNameTable;
    private static readonly DigimonExperienceTable = DigimonExperienceJson as DigimonExperienceTable;

    public static getRequiredExperienceForLevel(digimonName: string, level: number): number {
        return this.DigimonExperienceTable[digimonName]![level]!;
    }
    public static getNameById(id: number): string | null {
        return this.DigimonNameTable[id]!;
    }
}
