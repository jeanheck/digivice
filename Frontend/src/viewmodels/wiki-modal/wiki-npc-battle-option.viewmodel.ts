import type { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";

export interface WikiNpcBattleOptionViewModel {
  id: string;
  kind: NpcBattleKindConstant;
  battleId: string;
  charismaMin: number;
  charismaRangeText: string;
  completed: boolean;
}
