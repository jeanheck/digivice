import type { DigievolutionDTO } from './DigievolutionDTO';

export interface DigievolutionSlotDTO {
    index: number;
    digievolutionId?: number;
    digievolution?: DigievolutionDTO | null;
}
