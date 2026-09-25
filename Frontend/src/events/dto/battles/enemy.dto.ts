import type { VitalDTO } from "@/events/dto/parties/digimons/vital.dto";

export interface EnemyDTO {
  id?: number;
  groupId?: number;
  condition?: number;
  strength?: number;
  defense?: number;
  speed?: number;
  hp?: VitalDTO;
}
