import { DvexpThresholdLevelByTier } from "@/constants/digievolution.constant";
import type { Digimon, Party } from "@/models";
import { DigievolutionRepository } from "@/repositories/digievolution.repository";

const DvexpMultiplier = 10;
const DvexpLevelCap = 50;
const MinimumDvexp = 1;
const MaximumDvexp = 10;

export interface PartyDvexpGain {
  digimonId: number;
  dvexp: number | null;
}

export class DvexpService {
  private static isCapped(digievolutionId: number, digievolutionLevel: number): boolean {
    const tier = DigievolutionRepository.getTierById(digievolutionId);
    if (tier === 1) {
      return true;
    }

    return digievolutionLevel < DvexpThresholdLevelByTier[tier];
  }

  private static calculateActiveDigievolutionGain(
    baseDvexp: number,
    digimon: Digimon,
  ): number | null {
    const activeSlot = digimon.digievolutions.find((digievolutionSlot) => {
      return (
        digievolutionSlot.digievolutionId !== null &&
        digievolutionSlot.digievolutionId === digimon.activeDigievolutionId
      );
    });
    if (
      activeSlot === undefined ||
      activeSlot.digievolutionId === null ||
      activeSlot.digievolution === null
    ) {
      return null;
    }

    const isCapped = this.isCapped(activeSlot.digievolutionId, activeSlot.digievolution.level);

    return this.calculateBattleGain(baseDvexp, digimon.level, isCapped);
  }

  public static calculateBattleGain(
    baseDvexp: number,
    digimonLevel: number,
    isCapped: boolean,
  ): number {
    const dvexp = Math.max(
      MinimumDvexp,
      Math.floor((DvexpMultiplier * baseDvexp) / Math.min(digimonLevel, DvexpLevelCap)),
    );
    if (isCapped) {
      return Math.min(MaximumDvexp, dvexp);
    }

    return dvexp;
  }

  public static getPartyBattleGains(party: Party, baseDvexp: number): PartyDvexpGain[] {
    return party.slots.flatMap((slot) => {
      if (slot.digimonId === null || slot.digimon === null) {
        return [];
      }

      return [
        {
          digimonId: slot.digimonId,
          dvexp: this.calculateActiveDigievolutionGain(baseDvexp, slot.digimon),
        },
      ];
    });
  }
}
