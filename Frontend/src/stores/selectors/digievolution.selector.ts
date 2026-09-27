import { DvexpPerLevel } from "@/constants/digievolution.constant";

export class DigievolutionSelector {
  public static getDvexpProgress(dvexp: number): number {
    return dvexp % DvexpPerLevel;
  }
}
