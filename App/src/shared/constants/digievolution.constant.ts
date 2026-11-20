import type { DigievolutionTier } from "@/shared/types/digievolution-tier.type";

export const RegularDvexpPerLevel = 10;
export const ThresholdDvexpPerLevel = 50;
export const MaxDigievolutionLevel = 99;

export const DvexpThresholdLevelByTier = {
  1: 99,
  2: 95,
  3: 90,
  4: 80,
  5: 60,
} as const satisfies Record<DigievolutionTier, number>;
