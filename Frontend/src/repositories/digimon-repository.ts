import type { ExperienceByLevel } from "@/models/repository/experience-by-level";
import ExperienceTableData from "../database/ExperienceTable.json";

export class DigimonRepository {
    public static get experienceTable(): Record<string, ExperienceByLevel[]> {
        return (ExperienceTableData as unknown) as Record<string, ExperienceByLevel[]>;
    }

    public static getExperienceTableByDigimonName(digimonName: string): ExperienceByLevel[] | undefined {
        return this.experienceTable[digimonName];
    }

    public static getRequiredExperienceForLevel(digimonName: string, level: number): number {
        const experienceTable = this.getExperienceTableByDigimonName(digimonName);
        if (!experienceTable) {
            return 0;
        }

        const requiredExperience = experienceTable[level - 1];
        if (!requiredExperience) {
            return 0;
        }

        return requiredExperience[level] ?? 0;
    }

    public static getDigimonNameById(id: number): string {
        switch (id) {
            case 0: {
                return 'Kotemon';
            }
            case 1: {
                return 'Kumamon';
            }
            case 2: {
                return 'Monmon';
            }
            case 3: {
                return 'Agumon';
            }
            case 4: {
                return 'Veemon';
            }
            case 5: {
                return 'Guilmon';
            }
            case 6: {
                return 'Renamon';
            }
            case 7: {
                return 'Patamon';
            }
            default: {
                return 'Unknown';
            }
        }
    }
}
