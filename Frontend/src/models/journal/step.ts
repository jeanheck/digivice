import type { Requisite } from './requisite';

export interface Step {
    number: number;
    isDone: boolean;
    requisites: Requisite[];
}
