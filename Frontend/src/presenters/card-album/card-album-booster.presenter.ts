import type { CardInventory } from "@/models";
import { BoosterRepository } from "@/repositories/booster.repository";
import type {
  BoosterSeries,
  CardAlbumBoosterItemViewModel,
} from "@/viewmodels/card-album/card-album.viewmodel";

export interface BoosterCardTheme {
  wrapperClass: string;
  innerCardClass: string;
  crestClass: string;
  badgeClass: string;
  codeClass: string;
  progressClass: string;
  percentageClass: string;
}

export class CardAlbumBoosterPresenter {
  public static getBoosterSeries(boosterId: number): BoosterSeries {
    if (boosterId >= 501 && boosterId <= 515) {
      return "B"; // Série B: Vermelhas
    }
    if (boosterId >= 516 && boosterId <= 520) {
      return "R"; // Série R: Douradas
    }
    if (boosterId >= 521 && boosterId <= 535) {
      return "A"; // Série A: Azuis
    }
    return "B";
  }

  public static getBoosterCode(boosterId: number): string {
    if (boosterId >= 521 && boosterId <= 535) {
      const num = boosterId - 520;
      return `${String(num).padStart(2, "0")}a`;
    }
    if (boosterId >= 516 && boosterId <= 520) {
      const num = boosterId - 515;
      return `R-${String(num).padStart(2, "0")}`;
    }
    if (boosterId >= 501 && boosterId <= 515) {
      const num = boosterId - 500;
      return `${num}b`;
    }
    return String(boosterId);
  }

  public static getBoosterCardTheme(series: BoosterSeries): BoosterCardTheme {
    if (series === "R") {
      // R: Douradas (Gold)
      return {
        wrapperClass:
          "bg-linear-to-b from-[#332000] via-[#1a1000] to-[#0a0700] border-amber-500/70 hover:border-amber-300 hover:shadow-[0_0_22px_rgba(245,158,11,0.5)]",
        innerCardClass:
          "bg-linear-to-b from-amber-950/90 via-[#261700] to-black border-amber-400/80 text-amber-200 shadow-[0_0_12px_rgba(245,158,11,0.35)]",
        crestClass:
          "border-amber-300/80 bg-linear-to-b from-amber-400/30 to-amber-900/60 text-amber-200 shadow-[0_0_10px_rgba(245,158,11,0.6)]",
        badgeClass: "bg-amber-950/90 text-amber-200 border-amber-400/80",
        codeClass: "text-amber-400 font-mono",
        progressClass:
          "bg-linear-to-r from-amber-600 via-yellow-400 to-amber-300 shadow-[0_0_8px_rgba(245,158,11,0.5)]",
        percentageClass: "text-amber-300",
      };
    }

    if (series === "A") {
      // A: Azuis (Blue)
      return {
        wrapperClass:
          "bg-linear-to-b from-[#001f4d] via-[#000f26] to-[#000612] border-sky-500/70 hover:border-cyan-300 hover:shadow-[0_0_22px_rgba(14,165,233,0.5)]",
        innerCardClass:
          "bg-linear-to-b from-sky-950/90 via-[#001736] to-black border-cyan-400/80 text-cyan-200 shadow-[0_0_12px_rgba(6,182,212,0.35)]",
        crestClass:
          "border-cyan-300/80 bg-linear-to-b from-cyan-400/30 to-blue-900/60 text-cyan-200 shadow-[0_0_10px_rgba(6,182,212,0.6)]",
        badgeClass: "bg-cyan-950/90 text-cyan-200 border-cyan-400/80",
        codeClass: "text-cyan-400 font-mono",
        progressClass:
          "bg-linear-to-r from-blue-600 via-cyan-400 to-teal-300 shadow-[0_0_8px_rgba(6,182,212,0.5)]",
        percentageClass: "text-cyan-300",
      };
    }

    // B: Vermelhas (Red)
    return {
      wrapperClass:
        "bg-linear-to-b from-[#38040d] via-[#1c0207] to-[#0c0104] border-rose-500/70 hover:border-red-400 hover:shadow-[0_0_22px_rgba(244,63,94,0.5)]",
      innerCardClass:
        "bg-linear-to-b from-rose-950/90 via-[#28040b] to-black border-rose-500/80 text-rose-200 shadow-[0_0_12px_rgba(244,63,94,0.35)]",
      crestClass:
        "border-rose-400/80 bg-linear-to-b from-rose-500/30 to-rose-900/60 text-rose-200 shadow-[0_0_10px_rgba(244,63,94,0.6)]",
      badgeClass: "bg-rose-950/90 text-rose-200 border-rose-500/80",
      codeClass: "text-rose-400 font-mono",
      progressClass:
        "bg-linear-to-r from-rose-700 via-red-500 to-amber-500 shadow-[0_0_8px_rgba(244,63,94,0.5)]",
      percentageClass: "text-rose-300",
    };
  }

  private static readonly SERIES_ORDER: Record<BoosterSeries, number> = {
    A: 1,
    B: 2,
    R: 3,
  };

  public static getItems(cardInventory?: CardInventory): CardAlbumBoosterItemViewModel[] {
    const ids = BoosterRepository.getIds()
      .map(Number)
      .sort((a, b) => {
        const priorityA = this.SERIES_ORDER[this.getBoosterSeries(a)];
        const priorityB = this.SERIES_ORDER[this.getBoosterSeries(b)];
        if (priorityA !== priorityB) {
          return priorityA - priorityB;
        }
        return a - b;
      });
    const cardsInventory = cardInventory?.cards ?? {};

    return ids.map((boosterId) => {
      const cardIds = BoosterRepository.getCardIdsByBoosterId(boosterId);
      const totalCards = cardIds.length;
      const ownedCards = cardIds.filter((cid) => (cardsInventory[String(cid)] ?? 0) > 0).length;
      const isComplete = totalCards > 0 && ownedCards === totalCards;
      const percentage = totalCards > 0 ? ((ownedCards / totalCards) * 100).toFixed(0) : "0";

      return {
        id: boosterId,
        code: this.getBoosterCode(boosterId),
        nameKey: `boosters.${boosterId}.name`,
        series: this.getBoosterSeries(boosterId),
        totalCards,
        ownedCards,
        percentage,
        isComplete,
      };
    });
  }

  public static getSummary(cardInventory?: CardInventory): {
    totalBoosters: number;
    completedCount: number;
    percentage: string;
  } {
    const items = this.getItems(cardInventory);
    const totalBoosters = items.length;
    const completedCount = items.filter((b) => b.isComplete).length;
    const percentage = totalBoosters > 0 ? ((completedCount / totalBoosters) * 100).toFixed(1) : "0.0";

    return {
      totalBoosters,
      completedCount,
      percentage,
    };
  }
}
