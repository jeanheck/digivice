import experienceTable from '../data/static/ExperienceTable.json'

export class ExperienceCalculator {
    /**
     * Calculates the total absolute experience points required to reach the given absolute level
     * based on the provided Digimon Name.
     */
    static getRequiredExpForLevel(digimonName: string, level: number): number {
        if (level > 99) return 0;

        // Assert cast for JSON table access via unknown
        const table = (experienceTable as unknown as Record<string, Record<string, number>[]>)[digimonName]

        if (!table) {
            console.warn(`No static EXP table found for Digimon Name: ${digimonName}`)
            return 0
        }

        const levelStr = level.toString()
        const levelData = table.find(entry => levelStr in entry)

        return levelData ? (levelData[levelStr] || 0) : 0
    }

    /**
     * Helper to get the total absolute experience points required to reach the next level
     */
    static getRequiredExpForNextLevel(digimonName: string, currentLevel: number): number {
        return ExperienceCalculator.getRequiredExpForLevel(digimonName, currentLevel + 1)
    }

    /**
     * Helper to get the total absolute experience points required to reach the current level
     */
    static getRequiredExpForCurrentLevel(digimonName: string, currentLevel: number): number {
        return ExperienceCalculator.getRequiredExpForLevel(digimonName, currentLevel)
    }
}
