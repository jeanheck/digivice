import type { Requisite } from './requisite';

export interface Step {
    number: number;
    description?: string;
    isDone: boolean;
    requisites: Requisite[];
}
