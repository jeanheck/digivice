import type { DropType } from "@/presentation/repositories/tables/raws/drop/drop-type";

export interface EnemyDropRaw {
  dropId: number;
  type: DropType;
  locationOnly?: string;
}
