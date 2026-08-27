import type { NpcBattleKindConstant } from "@/constants/npc-battle-kind.constant";
import type { NpcBattleStatus } from "@/services/npc.service";
import type { NpcTrophyRequiredRaw } from "@/repositories/tables/raws/npc/npc-trophy-required.raw";

export interface WikiNpcBattleOptionViewModel {
  id: string;
  kind: NpcBattleKindConstant;
  battleId: string;
  charismaMin: number;
  charismaRangeText: string;
  completed: boolean;
  status: NpcBattleStatus;
  trophyRequired?: NpcTrophyRequiredRaw;
  requirementsMet: boolean;
  isActive: boolean;
  isSuperseded: boolean;
  missingRequirementTooltipKey: string | null;
  supersededTooltipKey: string | null;
  battleTooltipKey: string | null;
  showAsukaTrophyEmoji: boolean;
  asukaTrophyOwned: boolean;
}
