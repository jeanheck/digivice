import experienceTable from '../data/static/ExperienceTable.json'

export class ExperienceCalculator {
    /**
     * Calculates the total absolute experience points required to reach the next level
     * based on the provided Digimon Name and current Level.
     * 
     * @param digimonName Name identifying the Digimon species (used to lookup the right EXP arrays)
     * @param currentLevel The current absolute level of the Digimon (1 to 99)
     * @returns number The total EXP points required for currentLevel + 1. Returns 0 if max level (99).
     */
    static getRequiredExpForNextLevel(digimonName: string, currentLevel: number): number {
        // Level cap is 99
        if (currentLevel >= 99) {
            return 0
        }

        // Assert cast for JSON table access via unknown
        const table = (experienceTable as unknown as Record<string, Record<string, number>[]>)[digimonName]

        if (!table) {
            console.warn(`No static EXP table found for Digimon Name: ${digimonName}`)
            return 0
        }

        const nextLevelStr = (currentLevel + 1).toString()

        // Find the object in the array that has the next level as a key
        const nextLevelData = table.find(entry => nextLevelStr in entry)

        if (!nextLevelData) {
            return 0
        }

        return nextLevelData[nextLevelStr] || 0
    }
}
