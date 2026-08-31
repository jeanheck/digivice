import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";
import type { NpcBattleOpponentSearchKind } from "@/presenters/helper/npc-battle-opponent.helper";

export interface WikiNpcPanelViewModel {
  nameKey: string;
  searchKind: NpcBattleOpponentSearchKind;
  locationId: string;
  imageUrl: string | null;
  battleOptions: WikiNpcBattleOptionViewModel[];
}
