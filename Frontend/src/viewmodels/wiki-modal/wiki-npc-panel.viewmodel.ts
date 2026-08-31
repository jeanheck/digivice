import type { NpcTypeConstant } from "@/constants/npc-type.constant";
import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";

export interface WikiNpcPanelViewModel {
  nameKey: string;
  type: NpcTypeConstant;
  locationId: string;
  imageUrl: string | null;
  battleOptions: WikiNpcBattleOptionViewModel[];
}
