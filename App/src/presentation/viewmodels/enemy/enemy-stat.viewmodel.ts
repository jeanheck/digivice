export interface EnemyStatViewModel {
  statKey: string;
  value: number;
  icon: string;
  iconUrl: string | null;
  baseValue?: number;
  delta?: number;
}
