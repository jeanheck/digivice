import { DuelIslandRepository } from "@/repositories/duel-island.repository";
import { NpcRepository } from "@/repositories/npc.repository";
import { TamerRepository } from "@/repositories/tamer.repository";
import { ImageCatalog } from "@/catalogs/image.catalog";
import type { DuelIslandRaw } from "@/repositories/tables/raws/duel-island/duel-island.raw";
import type { NpcRaw } from "@/repositories/tables/raws/npc/npc.raw";
import type { TamerRaw } from "@/repositories/tables/raws/tamer/tamer.raw";

export type NpcBattleOpponent =
  | { source: "tamer"; raw: TamerRaw }
  | { source: "duelIsland"; raw: DuelIslandRaw }
  | { source: "npc"; raw: NpcRaw };

export type NpcBattleOpponentRaw = TamerRaw | DuelIslandRaw;

export type NpcBattleOpponentSearchKind = "tamer" | "leader" | "npc";

export class NpcBattleOpponentHelper {
  public static resolveById(id: string): NpcBattleOpponent | undefined {
    const tamerRaw = TamerRepository.getTamerById(id);
    if (tamerRaw !== undefined) {
      return { source: "tamer", raw: tamerRaw };
    }

    const duelIslandRaw = DuelIslandRepository.getDuelIslandById(id);
    if (duelIslandRaw !== undefined) {
      return { source: "duelIsland", raw: duelIslandRaw };
    }

    const npcRaw = NpcRepository.getNpcById(id);
    if (npcRaw !== undefined) {
      return { source: "npc", raw: npcRaw };
    }

    return undefined;
  }

  public static getById(opponentId: string): NpcBattleOpponentRaw | undefined {
    const opponent = this.resolveById(opponentId);
    if (opponent === undefined || opponent.source === "npc") {
      return undefined;
    }

    return opponent.raw;
  }

  public static getNameKey(opponentId: string): string | null {
    if (TamerRepository.getTamerById(opponentId) !== undefined) {
      return `tamers.${opponentId}.name`;
    }

    if (DuelIslandRepository.getDuelIslandById(opponentId) !== undefined) {
      return `duelIsland.${opponentId}.name`;
    }

    if (NpcRepository.getNpcById(opponentId) !== undefined) {
      return `npcs.${opponentId}.name`;
    }

    return null;
  }

  public static getSearchKind(opponentId: string): NpcBattleOpponentSearchKind | null {
    const opponent = this.resolveById(opponentId);
    if (opponent === undefined) {
      return null;
    }

    if (opponent.source === "tamer") {
      return "tamer";
    }

    if (opponent.source === "duelIsland") {
      return "npc";
    }

    return opponent.raw.type;
  }

  public static getImageUrl(opponentId: string): string | null {
    const opponent = this.resolveById(opponentId);
    if (opponent === undefined) {
      return null;
    }

    if (opponent.source === "npc") {
      return ImageCatalog.getNpcImageUrl(opponent.raw.imageName ?? null);
    }

    if (opponent.source === "duelIsland") {
      return ImageCatalog.getDuelIslandImageUrl(opponent.raw.imageName ?? null);
    }

    return ImageCatalog.getTamerImageUrl(opponent.raw.imageName ?? null);
  }

  public static getNpcIdByCardBattleId(cardBattleId: number): string | null {
    const tamerId = TamerRepository.getTamerIdByCardBattleId(cardBattleId);
    if (tamerId !== null) {
      return tamerId;
    }

    return DuelIslandRepository.getDuelIslandIdByCardBattleId(cardBattleId);
  }
}
