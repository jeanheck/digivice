import type { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";

export interface MapNpcViewModel {
  id: string;
  name: string;
  hasAvailableBattle: boolean;
  availableBattleKind: NpcBattleKindConstant | null;
}
