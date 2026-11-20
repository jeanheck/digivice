import type { Party } from "@/state/models";
import { PartyService } from "@/presentation/services/party.service";

export class FooterPresenter {
  public static getPartyLevel(party: Party): number {
    return PartyService.getLevel(party);
  }

  public static getPartyCharisma(party: Party): number {
    return PartyService.getCharisma(party);
  }
}
