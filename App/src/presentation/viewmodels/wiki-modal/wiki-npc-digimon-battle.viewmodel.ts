import type { EnemyViewModel } from "@/presentation/viewmodels/enemy/enemy.viewmodel";

export interface WikiNpcPartyMemberViewModel {
  id: string;
  enemy: EnemyViewModel;
  imageUrl: string | null;
}

export interface WikiNpcDigimonBattleViewModel {
  exp: number | null;
  dvexp: number | null;
  bits: number;
  partyMemberCount: number;
  members: WikiNpcPartyMemberViewModel[];
}
