import type { Quest } from "./quest";

export interface Journal {
  mainQuest: Quest;
  sideQuests: Quest[];
  legendaryWeapons: Quest[];
  driAgents: Quest[];
  duelIsland: Quest[];
}
