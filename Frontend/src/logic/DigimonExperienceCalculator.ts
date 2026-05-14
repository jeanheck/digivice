import experienceTableJson from '../database/ExperienceTable.json';
import { MathUtils } from '../utils/MathUtils';

interface ExperienceEntry {
    [level: string]: number;
}

type RawExperienceTable = Record<string, ExperienceEntry[]>;

export class DigimonExperienceCalculator {
    public static readonly MAX_LEVEL = 99;

    private static experienceIndexedTable: Map<string, number[]> = new Map();

    private static ensureIndexed() {
        if (this.experienceIndexedTable.size > 0) {
            return;
        }

        const rawData = experienceTableJson as unknown as RawExperienceTable;

        for (const [digimonName, experienceByLevel] of Object.entries(rawData)) {
            const experienceArray = new Array(this.MAX_LEVEL).fill(0);

            experienceByLevel.forEach(entry => {
                const levelString = Object.keys(entry)[0];
                if (!levelString) {
                    return;
                }

                const level = parseInt(levelString);
                if (level >= 1 && level <= this.MAX_LEVEL) {
                    experienceArray[level - 1] = entry[levelString];
                }
            });

            this.experienceIndexedTable.set(digimonName, experienceArray);
        }
    }

    public static getRequiredExpForLevel(digimonName: string, level: number): number {
        this.ensureIndexed();

        const digimonExperienceTable = this.experienceIndexedTable.get(digimonName);
        if (!digimonExperienceTable) {
            console.warn(`[Experience] No table found for: ${digimonName}`);
            return 0;
        }

        // Ensure the level is between 1 and 99
        const sanitizedLevel = Math.max(1, Math.min(level, this.MAX_LEVEL));

        return digimonExperienceTable[sanitizedLevel - 1] ?? 0;
    }

    public static getRequiredExpForNextLevel(digimonName: string, currentLevel: number): number {
        return this.getRequiredExpForLevel(digimonName, currentLevel + 1);
    }

    public static getRequiredExpForCurrentLevel(digimonName: string, currentLevel: number): number {
        return this.getRequiredExpForLevel(digimonName, currentLevel);
    }

    public static getProgressPercentageForNextLevel(digimonName: string, currentLevel: number, currentExp: number): number {
        if (currentLevel >= this.MAX_LEVEL) {
            return 100;
        }

        const requiredExpForCurrentLevel = this.getRequiredExpForCurrentLevel(digimonName, currentLevel);
        const requiredExpForNextLevel = this.getRequiredExpForNextLevel(digimonName, currentLevel);

        const totalRequiredInThisLevel = requiredExpForNextLevel - requiredExpForCurrentLevel;
        const progressInThisLevel = currentExp - requiredExpForCurrentLevel;
        
        return MathUtils.calculatePercentage(progressInThisLevel, totalRequiredInThisLevel);
    }
}
