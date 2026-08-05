import type { VitalDTO } from "./vital.dto";

export interface InCombatDTO {
  condition?: number;
  hp?: VitalDTO;
  mp?: VitalDTO;
}
