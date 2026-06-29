import type { RequisiteDTO } from './requisite.dto';

export interface StepDTO {
    number: number;
    value?: number; // Representa o byte bruto
    requisites?: RequisiteDTO[];
}
