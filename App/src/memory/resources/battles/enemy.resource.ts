import type { VitalResource } from "@/memory/resources/parties/digimons/vital.resource";

export interface EnemyResource {
  id: number;
  groupId: number;
  condition: number;
  strength: number;
  defense: number;
  speed: number;
  hp: VitalResource;
}
