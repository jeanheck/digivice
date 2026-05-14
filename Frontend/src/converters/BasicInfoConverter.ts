import { DigimonExperienceCalculator } from '../logic/DigimonExperienceCalculator';
import type { BasicInfo } from '../models/Digimon';

export class BasicInfoConverter {
    /**
     * Creates a new enriched BasicInfo object by merging the current state with the incoming delta overrides.
     */
    public static convert(current: BasicInfo, overrides: Partial<BasicInfo>): BasicInfo {
        // Clone the current object and apply the delta overrides
        const updated = { ...current, ...overrides };
        
        // Always apply enrichment rules (recalculate experience data)
        updated.experienceToReachNextLevel = DigimonExperienceCalculator.getRequiredExpForNextLevel(
            updated.name, 
            updated.level
        );
        
        updated.experiencePercentageToReachNextLevel = DigimonExperienceCalculator.getProgressPercentageForNextLevel(
            updated.name, 
            updated.level, 
            updated.experience
        );
        
        return updated;
    }
}
