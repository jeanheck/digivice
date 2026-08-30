import type { Auction } from "./auction";
import type { Npc } from "./npc";
import type { Quest } from "./quest";

export interface Journal {
  mainQuest: Quest | null;
  sideQuests: Quest[];
  legendaryWeapons: Quest[];
  driAgents: Quest[];
  duelIsland: Quest[];
  auctions: Auction[];
  npcs: Npc[];
}
