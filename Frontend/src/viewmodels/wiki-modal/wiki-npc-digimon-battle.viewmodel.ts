import type { EnemyViewModel } from "@/viewmodels/enemy/enemy.viewmodel";

export interface WikiNpcPartyMemberViewModel {
  id: string;
  enemy: EnemyViewModel;
  imageUrl: string | null;
}

export interface WikiNpcDigimonBattleViewModel {
  exp: number;
  dvexp: number;
  bits: number;
  partyMemberCount: number;
  members: WikiNpcPartyMemberViewModel[];
}
