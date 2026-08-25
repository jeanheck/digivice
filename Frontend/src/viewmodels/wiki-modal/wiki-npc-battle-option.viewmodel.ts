import type { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import type { NpcBattleStatus } from "@/services/npc.service";

export interface WikiNpcBattleOptionViewModel {
  id: string;
  kind: NpcBattleKindConstant;
  battleId: string;
  charismaMin: number;
  charismaRangeText: string;
  completed: boolean;
  status: NpcBattleStatus;
}
