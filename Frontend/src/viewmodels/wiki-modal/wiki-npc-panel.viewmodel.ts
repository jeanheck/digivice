import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";
import type { NpcType } from "@/types/npc-type.type";

export interface WikiNpcPanelViewModel {
  nameKey: string;
  searchKind: NpcType;
  locationId: string;
  imageUrl: string | null;
  battleOptions: WikiNpcBattleOptionViewModel[];
}
