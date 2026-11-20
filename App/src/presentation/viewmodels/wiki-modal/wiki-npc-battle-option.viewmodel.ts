import type { NpcBattleKind } from "@/shared/types/npc-battle-kind.type";
import type { NpcBattleStatus } from "@/presentation/services/npc.service";
import type { TamerTrophyRequiredRaw } from "@/presentation/repositories/tables/raws/tamer/tamer-trophy-required.raw";

export interface WikiNpcBattleOptionViewModel {
  id: string;
  kind: NpcBattleKind;
  battleId: string;
  charismaMin: number;
  charismaRangeText: string;
  won: boolean;
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
