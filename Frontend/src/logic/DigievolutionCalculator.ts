import { DigievolutionRepository } from '@/repositories/digievolution-repository';
import type { EnrichedDigievolution, Digimon, DigievolutionSlot } from '@/models';

export class DigievolutionCalculator {
    public static getDigievolutionNameById(id: number): string {
        return DigievolutionRepository.getDigievolutionNameById(id);
    }

    public static getActiveEnrichedDigievolution(digievolutions: DigievolutionSlot[], activeDigievolutionId: number): EnrichedDigievolution | null {
        const activeDigievolutionSlot = digievolutions.find(slot => slot.digievolutionId === activeDigievolutionId);
        if(!activeDigievolutionSlot){
            return null;
        }
        const activeDigievolution = activeDigievolutionSlot.digievolution;
        if(!activeDigievolution){
            return null;
        }

        return DigievolutionRepository.getEnrichedDigievolution(activeDigievolutionId, activeDigievolution.level);
    }
}
