import type { DigimonExperienceTable } from "@/models/repository/digimon-experience-table";
import type { DigimonNameTable } from "@/models/repository/digimon-name-table";
import DigimonExperience from "@/tables/digimon/digimon-experience.json";
import DigimonName from "@/tables/digimon/digimon-name.json";

export class DigimonRepository {
    private static readonly DigimonNameData = DigimonName as DigimonNameTable;
    private static readonly DigimonExperienceData = DigimonExperience as DigimonExperienceTable;

    public static getRequiredExperienceForLevel(digimonName: string, level: number): number {
        return this.DigimonExperienceData[digimonName]![level]!;
    }
    public static getNameById(id: number): string | null {
        return this.DigimonNameData[id]!;
    }
}
