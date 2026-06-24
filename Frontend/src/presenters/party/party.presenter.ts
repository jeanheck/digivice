import type { DigimonSlot } from "@/models/party/digimon-slot";

export class PartyPresenter {
    public static getFilledSlots(slots: DigimonSlot[] | undefined | null): DigimonSlot[] {
        return (slots ?? []).filter(slot => slot.digimonId !== null && slot.digimon !== null);
    }
}
