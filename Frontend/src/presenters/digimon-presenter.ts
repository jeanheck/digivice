import { DigimonExperienceCalculator } from "@/logic/DigimonExperienceCalculator";
import { DigimonRepository } from "@/repositories/digimon-repository";

export class DigimonPresenter {
    public static getDigimonNameById(digimonId: number): string {
        return DigimonRepository.getNameById(digimonId);
    }
    public static getRequiredExperienceForNextLevel(digimonName: string, level: number): number {
        return DigimonExperienceCalculator.getRequiredExperienceForNextLevel(digimonName, level);
    }
    public static getProgressPercentageForNextLevel(digimonName: string, level: number, experience: number): number {
        return DigimonExperienceCalculator.getProgressPercentageForNextLevel(digimonName, level, experience);
    }
}
