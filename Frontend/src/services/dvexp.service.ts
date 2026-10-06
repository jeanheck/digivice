import type { Party } from "@/models";

const DvexpMultiplier = 10;
const DvexpLevelCap = 50;
const MinimumDvexp = 1;
const MaximumDvexp = 10;

export interface PartyDvexpGain {
  digimonId: number;
  dvexp: number;
}

export class DvexpService {
  public static calculateBattleGain(baseDvexp: number, digimonLevel: number): number {
    const dvexp = Math.floor(
      (DvexpMultiplier * baseDvexp) / Math.min(digimonLevel, DvexpLevelCap),
    );

    return Math.min(MaximumDvexp, Math.max(MinimumDvexp, dvexp));
  }

  public static getPartyBattleGains(party: Party, baseDvexp: number): PartyDvexpGain[] {
    return party.slots.flatMap((slot) => {
      if (slot.digimonId === null || slot.digimon === null) {
        return [];
      }

      return [
        {
          digimonId: slot.digimonId,
          dvexp: this.calculateBattleGain(baseDvexp, slot.digimon.level),
        },
      ];
    });
  }
}
