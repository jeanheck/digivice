import type { Digimon, DigimonSlot, Party } from "@/state/models";

type OccupiedDigimonSlot = DigimonSlot & { digimonId: number; digimon: Digimon };

const MinimumLevel = 1;
const MaximumLevel = 99;

export class SaveLoadedValidator {
  private static isOccupied(slot: DigimonSlot): slot is OccupiedDigimonSlot {
    return slot.digimonId !== null && slot.digimon !== null;
  }

  public static findViolation(party: Party): string | null {
    const occupiedSlots = party.slots.filter((slot) => SaveLoadedValidator.isOccupied(slot));
    if (occupiedSlots.length === 0) {
      return "No occupied party slot.";
    }

    const digimonIds = occupiedSlots.map((slot) => slot.digimonId);
    if (new Set(digimonIds).size !== digimonIds.length) {
      return `Repeated digimonId in the party (${digimonIds.join(", ")}).`;
    }

    const slotWithInvalidLevel = occupiedSlots.find(
      (slot) => slot.digimon.level < MinimumLevel || slot.digimon.level > MaximumLevel,
    );
    if (slotWithInvalidLevel) {
      return `Party slot ${slotWithInvalidLevel.index} has level ${slotWithInvalidLevel.digimon.level}.`;
    }

    return null;
  }
}
