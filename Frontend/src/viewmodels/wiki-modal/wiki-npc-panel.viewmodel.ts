import type { WikiNpcBattleOptionViewModel } from "@/viewmodels/wiki-modal/wiki-npc-battle-option.viewmodel";

export interface WikiNpcPanelViewModel {
  nameKey: string;
  searchKind: "tamer" | "npc";
  locationId: string;
  imageUrl: string | null;
  battleOptions: WikiNpcBattleOptionViewModel[];
}
