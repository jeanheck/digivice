import { MathHelper } from "@/presenters/helper/math.helper";
import { DigimonRepository } from "@/repositories/digimon.repository";
import type { ExperienceProgressBarViewModel } from "@/viewmodels/party/digimon/profile/experience-progress-bar.viewmodel";

export class ExperienceProgressBarPresenter {
    private static readonly MAX_LEVEL = 99;

    public static getCalculatedExperienceValues(
        digimonId: number,
        level: number,
        experience: number,
    ): ExperienceProgressBarViewModel {
        if (level === this.MAX_LEVEL) {
            return {
                maxValue: 0,
                percentage: 100,
            };
        }

        const requiredExperienceForNextLevel = DigimonRepository.getRequiredExperienceForLevel(digimonId, level + 1);
        const requiredExperienceForCurrentLevel = DigimonRepository.getRequiredExperienceForLevel(digimonId,level,);
        const totalRequiredInThisLevel = requiredExperienceForNextLevel - requiredExperienceForCurrentLevel;
        const progressInThisLevel = experience - requiredExperienceForCurrentLevel;

        return {
            maxValue: requiredExperienceForNextLevel,
            percentage: MathHelper.calculatePercentage(
                progressInThisLevel,
                totalRequiredInThisLevel,
            ),
        };
    }
}
