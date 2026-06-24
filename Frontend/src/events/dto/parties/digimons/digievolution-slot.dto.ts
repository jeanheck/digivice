import type { DigievolutionDTO } from './digievolution.dto';

export interface DigievolutionSlotDTO {
    index: number;
    digievolutionId?: number;
    digievolution?: DigievolutionDTO | null;
}
