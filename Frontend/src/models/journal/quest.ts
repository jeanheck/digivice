import type { Requisite } from './requisite';
import type { Step } from './step';

export interface Quest {
    id: string;
    requisites: Requisite[];
    steps: Step[];
}
