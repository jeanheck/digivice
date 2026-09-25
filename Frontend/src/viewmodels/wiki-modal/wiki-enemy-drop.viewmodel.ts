import type { DropType } from "@/repositories/tables/raws/drop/drop-type";

export interface WikiEnemyDropViewModel {
  dropId: number;
  type: DropType;
  labelKey: string;
  locationOnly?: string;
}
