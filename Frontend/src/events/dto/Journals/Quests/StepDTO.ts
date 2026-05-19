import type { RequisiteDTO } from './RequisiteDTO';

export interface StepDTO {
    number: number;
    value?: number; // Representa o byte bruto
    requisites?: RequisiteDTO[];
}
