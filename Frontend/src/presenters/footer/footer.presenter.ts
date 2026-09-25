import type { Party } from "@/models";
import { PartyService } from "@/services/party.service";

export class FooterPresenter {
  public static getPartyLevel(party: Party): number {
    return PartyService.getLevel(party);
  }

  public static getPartyCharisma(party: Party): number {
    return PartyService.getCharisma(party);
  }
}
