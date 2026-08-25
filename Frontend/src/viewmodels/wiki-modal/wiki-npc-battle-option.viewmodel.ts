import type { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";

export interface WikiNpcBattleOptionViewModel {
  id: string;
  kind: NpcBattleKindConstant;
  battleIndex: number;
  charismaMin: number;
  charismaRangeText: string;
}
