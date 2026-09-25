import type { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import type { NpcBattleStatus } from "@/services/npc.service";
import type { TamerTrophyRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-trophy-required.raw";

export interface WikiNpcBattleOptionViewModel {
  id: string;
  kind: NpcBattleKindConstant;
  battleId: string;
  charismaMin: number;
  charismaRangeText: string;
  completed: boolean;
  status: NpcBattleStatus;
  trophyRequired?: TamerTrophyRequiredRaw;
  requirementsMet: boolean;
  isActive: boolean;
  isSuperseded: boolean;
  missingRequirementTooltipKey: string | null;
  supersededTooltipKey: string | null;
  battleTooltipKey: string | null;
  showTrophyEmoji: boolean;
  trophyOwned: boolean;
}
