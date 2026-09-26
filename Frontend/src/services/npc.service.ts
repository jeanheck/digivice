import type { ImportantItems, Npc } from "@/models";
import { StoryNpcDigimonBattleId } from "@/constants/npc-battle.constant";
import type { NpcBattleKind } from "@/types/npc-battle-kind.type";
import type {
  NpcBattleOpponent,
  NpcBattleOpponentRaw,
} from "@/presenters/helper/npc-battle-opponent.helper";
import type { TamerCardBattleRaw } from "@/repositories/tables/raws/tamer/tamer-card-battle.raw";
import type { TamerCharismaRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-charisma-required.raw";
import type { TamerTrophyRequiredRaw } from "@/repositories/tables/raws/tamer/tamer-trophy-required.raw";

export type NpcBattleStatus = "completed" | "available" | "missingRequirements";

const MissingCharismaTooltipKey = "npc.battle.requirement.missingCharisma";
const MissingAsukaTrophyTooltipKey = "npc.battle.requirement.missingAsukaTrophy";
const MissingSunTrophyTooltipKey = "npc.battle.requirement.missingSunTrophy";
const MissingCharismaAndAsukaTrophyTooltipKey =
  "npc.battle.requirement.missingCharismaAndAsukaTrophy";
const MissingCharismaAndSunTrophyTooltipKey =
  "npc.battle.requirement.missingCharismaAndSunTrophy";
const UnavailableAfterAsukaTrophyTooltipKey =
  "npc.battle.requirement.unavailableAfterAsukaTrophy";
const UnavailableAfterSunTrophyTooltipKey =
  "npc.battle.requirement.unavailableAfterSunTrophy";
const AvailableBattleTooltipKey = "npc.battle.requirement.available";
const AlreadyWonBattleTooltipKey = "npc.battle.requirement.alreadyWon";

export class NpcService {
  public static isCharismaInRange(
    partyCharisma: number,
    charismaRequired: TamerCharismaRequiredRaw,
  ): boolean {
    if (partyCharisma < charismaRequired.min) {
      return false;
    }

    if (charismaRequired.max !== undefined && partyCharisma > charismaRequired.max) {
      return false;
    }

    return true;
  }

  public static isTrophyRequirementMet(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    importantItems: ImportantItems,
  ): boolean {
    if (trophyRequired === undefined) {
      return true;
    }

    if (trophyRequired === "asukaTrophy") {
      return importantItems.asukaTrophy;
    }

    if (trophyRequired === "sunTrophy") {
      return importantItems.sunTrophy;
    }

    return false;
  }

  private static getMissingTrophyTooltipKey(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
  ): string | null {
    if (trophyRequired === "asukaTrophy") {
      return MissingAsukaTrophyTooltipKey;
    }

    if (trophyRequired === "sunTrophy") {
      return MissingSunTrophyTooltipKey;
    }

    return null;
  }

  private static getMissingCharismaAndTrophyTooltipKey(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
  ): string {
    if (trophyRequired === "sunTrophy") {
      return MissingCharismaAndSunTrophyTooltipKey;
    }

    return MissingCharismaAndAsukaTrophyTooltipKey;
  }

  public static getUnavailableAfterTrophyTooltipKey(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
  ): string {
    if (trophyRequired === "sunTrophy") {
      return UnavailableAfterSunTrophyTooltipKey;
    }

    return UnavailableAfterAsukaTrophyTooltipKey;
  }

  public static isTrophyOwned(
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    importantItems: ImportantItems,
  ): boolean {
    return this.isTrophyRequirementMet(trophyRequired, importantItems);
  }

  public static areBattleRequirementsMet(
    charismaRequired: TamerCharismaRequiredRaw,
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    partyCharisma: number,
    importantItems: ImportantItems,
  ): boolean {
    if (!this.isCharismaInRange(partyCharisma, charismaRequired)) {
      return false;
    }

    return this.isTrophyRequirementMet(trophyRequired, importantItems);
  }

  public static getMissingRequirementTooltipKey(
    charismaRequired: TamerCharismaRequiredRaw,
    trophyRequired: TamerTrophyRequiredRaw | undefined,
    partyCharisma: number,
    importantItems: ImportantItems,
  ): string | null {
    const charismaMet = this.isCharismaInRange(partyCharisma, charismaRequired);
    const trophyMet = this.isTrophyRequirementMet(trophyRequired, importantItems);

    if (charismaMet && trophyMet) {
      return null;
    }

    if (!charismaMet && !trophyMet) {
      return this.getMissingCharismaAndTrophyTooltipKey(trophyRequired);
    }

    if (!charismaMet) {
      return MissingCharismaTooltipKey;
    }

    return this.getMissingTrophyTooltipKey(trophyRequired);
  }

  public static resolveActiveCardBattleIds(
    cardBattles: Record<string, TamerCardBattleRaw> | undefined,
    partyCharisma: number,
    importantItems: ImportantItems,
  ): Set<string> {
    const sortedCardBattles = Object.entries(cardBattles ?? {}).sort(
      ([firstBattleId], [secondBattleId]) => {
        return firstBattleId.localeCompare(secondBattleId);
      },
    );

    let activeBattleId: string | null = null;

    for (const [battleId, cardBattle] of sortedCardBattles) {
      if (
        this.areBattleRequirementsMet(
          cardBattle.charismaRequired,
          cardBattle.trophyRequired,
          partyCharisma,
          importantItems,
        )
      ) {
        activeBattleId = battleId;
      }
    }

    if (activeBattleId === null) {
      return new Set();
    }

    return new Set([activeBattleId]);
  }

  public static isDigimonBattleCompleted(
    journalNpc: Npc | null | undefined,
    battleId: string,
  ): boolean {
    if (journalNpc === null || journalNpc === undefined) {
      return false;
    }

    const battle = journalNpc.battles.find((entry) => {
      return entry.id === battleId;
    });

    return battle?.won ?? false;
  }

  public static getBattleTooltipKey(params: {
    status: NpcBattleStatus;
    isSuperseded: boolean;
    missingRequirementTooltipKey: string | null;
    supersededTooltipKey: string | null;
  }): string | null {
    if (params.status === "completed") {
      return AlreadyWonBattleTooltipKey;
    }

    if (params.status === "available") {
      return AvailableBattleTooltipKey;
    }

    if (params.isSuperseded) {
      return params.supersededTooltipKey ?? UnavailableAfterAsukaTrophyTooltipKey;
    }

    return params.missingRequirementTooltipKey;
  }

  public static getBattleStatus(completed: boolean, isActive: boolean): NpcBattleStatus {
    if (completed) {
      return "completed";
    }

    if (isActive) {
      return "available";
    }

    return "missingRequirements";
  }

  public static getAvailableBattleKind(
    opponent: NpcBattleOpponentRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems,
  ): NpcBattleKind | null {
    return this.getAvailableBattleKindFromTamerOrDuelIsland(
      opponent,
      journalNpc,
      partyCharisma,
      importantItems,
    );
  }

  public static getAvailableBattleKindForOpponent(
    opponent: NpcBattleOpponent,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems,
  ): NpcBattleKind | null {
    if (opponent.source === "npc") {
      const completed = this.isDigimonBattleCompleted(journalNpc, StoryNpcDigimonBattleId);
      if (completed) {
        return null;
      }

      return "digimon";
    }

    return this.getAvailableBattleKindFromTamerOrDuelIsland(
      opponent.raw,
      journalNpc,
      partyCharisma,
      importantItems,
    );
  }

  private static getAvailableBattleKindFromTamerOrDuelIsland(
    tamer: NpcBattleOpponentRaw,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems,
  ): NpcBattleKind | null {
    const hasAvailableDigimonBattle = Object.entries(tamer.digimonBattles ?? {}).some(
      ([battleId, digimonBattle]) => {
        const completed = this.isDigimonBattleCompleted(journalNpc, battleId);
        if (completed) {
          return false;
        }

        return this.areBattleRequirementsMet(
          digimonBattle.charismaRequired,
          digimonBattle.trophyRequired,
          partyCharisma,
          importantItems,
        );
      },
    );

    if (hasAvailableDigimonBattle) {
      return "digimon";
    }

    const activeCardBattleIds = this.resolveActiveCardBattleIds(
      tamer.cardBattles,
      partyCharisma,
      importantItems,
    );
    if (activeCardBattleIds.size > 0) {
      return "card";
    }

    return null;
  }

  public static hasAvailableBattle(
    opponent: NpcBattleOpponent,
    journalNpc: Npc | null | undefined,
    partyCharisma: number,
    importantItems: ImportantItems,
  ): boolean {
    return (
      this.getAvailableBattleKindForOpponent(
        opponent,
        journalNpc,
        partyCharisma,
        importantItems,
      ) !== null
    );
  }
}
