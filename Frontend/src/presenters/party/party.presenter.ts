import type { DigimonSlot } from "@/models";

export class PartyPresenter {
  public static getFilledSlots(slots: DigimonSlot[]): DigimonSlot[] {
    return slots.filter((slot) => slot.digimonId !== null && slot.digimon !== null);
  }
}
