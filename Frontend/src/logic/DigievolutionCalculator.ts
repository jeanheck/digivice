import { DigievolutionRepository } from '@/repositories/digievolution-repository';
import type { DigievolutionRaw } from '@/repositories/tables/raws/digievolution/digievolution-raw';

export class DigievolutionCalculator {
    public static getDigievolutionNameById(id: number): string {
        return DigievolutionRepository.getNameById(id);
    }

    public static getActiveRawDigievolution(activeDigievolutionId: number): DigievolutionRaw {
        return DigievolutionRepository.getRawDigievolutionById(activeDigievolutionId);
    }
}
