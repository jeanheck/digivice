import type { Digievolution } from './digievolution';

export interface DigievolutionSlot {
    index: number;
    digievolutionId: number;
    digievolution: Digievolution | null;
}
