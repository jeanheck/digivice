import type { NpcTypeConstant } from "@/constants/npc-type.constant";
import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";

export interface WikiNpcPanelViewModel {
  name: string;
  type: NpcTypeConstant;
  locationId: string;
  battleOptions: WikiNpcBattleOptionViewModel[];
}
