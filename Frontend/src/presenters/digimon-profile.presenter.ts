import { DigimonRepository } from "@/repositories/digimon.repository";
import { MathUtils } from "@/utils/MathUtils";

export class ProfilePresenter {
    private static readonly MAX_LEVEL = 99;

    public static getNameById(id: number): string {
        return DigimonRepository.getNameById(id);
    }

    public static getRequiredExperienceForNextLevel(id: number, level: number): number {
        if (level === this.MAX_LEVEL) {
            return 0;
        }

        return DigimonRepository.getRequiredExperienceForLevel(id, level + 1);
    }

    public static calculateProgressPercentageForNextLevel(id: number, level: number, experience: number): number {
        if (level === this.MAX_LEVEL) {
            return 100;
        }

        const requiredExperienceForCurrentLevel = DigimonRepository.getRequiredExperienceForLevel(id, level);
        const requiredExperienceForNextLevel = this.getRequiredExperienceForNextLevel(id, level);
        const totalRequiredInThisLevel = requiredExperienceForNextLevel - requiredExperienceForCurrentLevel;
        const progressInThisLevel = experience - requiredExperienceForCurrentLevel;

        return MathUtils.calculatePercentage(progressInThisLevel, totalRequiredInThisLevel);
    }
}
