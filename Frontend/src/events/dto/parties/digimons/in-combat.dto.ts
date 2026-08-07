import type { VitalDTO } from "./vital.dto";

export interface InCombatDTO {
  condition?: number;
  strength?: number;
  defense?: number;
  speed?: number;
  hp?: VitalDTO;
  mp?: VitalDTO;
}
