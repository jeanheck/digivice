import { DigimonRepository } from '@/repositories/digimon-repository';
import { MathUtils } from '../utils/MathUtils';

export class DigimonExperienceCalculator {
    public static readonly MAX_LEVEL = 99;

    public static getRequiredExperienceForCurrentLevel(digimonName: string, currentLevel: number): number {
        return DigimonRepository.getRequiredExperienceForLevel(digimonName, currentLevel);
    }

    public static getRequiredExperienceForNextLevel(digimonName: string, currentLevel: number): number {
        if (currentLevel >= this.MAX_LEVEL) {
            return 0;
        }
        return DigimonRepository.getRequiredExperienceForLevel(digimonName, currentLevel + 1);
    }

    public static getProgressPercentageForNextLevel(digimonName: string, currentLevel: number, currentExp: number): number {
        if (currentLevel >= this.MAX_LEVEL) {
            return 100;
        }

        const requiredExpForCurrentLevel = this.getRequiredExperienceForCurrentLevel(digimonName, currentLevel);
        const requiredExpForNextLevel = this.getRequiredExperienceForNextLevel(digimonName, currentLevel);

        const totalRequiredInThisLevel = requiredExpForNextLevel - requiredExpForCurrentLevel;
        const progressInThisLevel = currentExp - requiredExpForCurrentLevel;
        
        return MathUtils.calculatePercentage(progressInThisLevel, totalRequiredInThisLevel);
    }
}
