import type { Party } from "@/models";

export class PartyHelper {
  public static getLevel(party: Party): number {
    const digimons = party.slots
      .map((slot) => slot.digimon)
      .filter((digimon) => digimon !== null);

    return Math.sum(
      digimons.map((digimon) => {
        return digimon.level;
      }),
    );
  }
}
