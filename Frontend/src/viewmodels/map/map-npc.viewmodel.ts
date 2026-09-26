import type { NpcBattleKind } from "@/types/npc-battle-kind.type";

export interface MapNpcViewModel {
  id: string;
  nameKey: string;
  hasAvailableBattle: boolean;
  availableBattleKind: NpcBattleKind | null;
}
