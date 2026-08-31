import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import type { DuelIslandRaw } from "@/repositories/tables/raws/duel-island/duel-island.raw";
import type { TamerRaw } from "@/repositories/tables/raws/tamer/tamer.raw";

export type NpcBattleOpponentRaw = TamerRaw | DuelIslandRaw;

export type NpcBattleOpponentSearchKind = "tamer" | "npc";

export class NpcBattleOpponentHelper {
  public static getById(opponentId: string): NpcBattleOpponentRaw | undefined {
    const tamerRaw = TamerRepository.getTamerById(opponentId);
    if (tamerRaw !== undefined) {
      return tamerRaw;
    }

    return DuelIslandRepository.getDuelIslandById(opponentId);
  }

  public static getNameKey(opponentId: string): string | null {
    if (TamerRepository.getTamerById(opponentId) !== undefined) {
      return `tamers.${opponentId}.name`;
    }

    if (DuelIslandRepository.getDuelIslandById(opponentId) !== undefined) {
      return `duelIsland.${opponentId}.name`;
    }

    return null;
  }

  public static getSearchKind(opponentId: string): NpcBattleOpponentSearchKind | null {
    if (TamerRepository.getTamerById(opponentId) !== undefined) {
      return "tamer";
    }

    if (DuelIslandRepository.getDuelIslandById(opponentId) !== undefined) {
      return "npc";
    }

    return null;
  }

  public static getIdByOpponentId(opponentId: number): string | null {
    const tamerId = TamerRepository.getTamerIdByOpponentId(opponentId);
    if (tamerId !== null) {
      return tamerId;
    }

    return DuelIslandRepository.getDuelIslandIdByOpponentId(opponentId);
  }
}
