import type { Npcs } from "@/state/models";

export interface NpcBattleResource {
  id: string;
  value: number;
}

export interface NpcResource {
  battles: NpcBattleResource[];
}

export type NpcsResource = Record<keyof Npcs, NpcResource>;
