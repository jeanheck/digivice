import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";

export interface WikiEnemyDropViewModel {
  dropId: number;
  type: DropType;
  labelKey: string;
  locationOnly?: string;
}
