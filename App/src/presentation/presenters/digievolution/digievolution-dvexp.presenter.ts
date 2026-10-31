import {
  DvexpThresholdLevelByTier,
  MaxDigievolutionLevel,
  RegularDvexpPerLevel,
  ThresholdDvexpPerLevel,
} from "@/shared/constants/digievolution.constant";
import { DigievolutionRepository } from "@/presentation/repositories/digievolution.repository";

export interface DvexpProgress {
  current: number;
  max: number;
}

export class DigievolutionDvexpPresenter {
  public static getProgress(digievolutionId: number, level: number, dvexp: number): DvexpProgress {
    const tier = DigievolutionRepository.getTierById(digievolutionId);
    const thresholdLevel = DvexpThresholdLevelByTier[tier];
    const isThresholdRegime = level >= thresholdLevel;
    const max = isThresholdRegime ? ThresholdDvexpPerLevel : RegularDvexpPerLevel;

    if (level >= MaxDigievolutionLevel) {
      return { current: max, max };
    }

    if (!isThresholdRegime) {
      return { current: dvexp % RegularDvexpPerLevel, max };
    }

    const thresholdDvexp = (thresholdLevel - 1) * RegularDvexpPerLevel;
    return { current: (dvexp - thresholdDvexp) % ThresholdDvexpPerLevel, max };
  }
}
