import type { Party } from "@/models";
import { PartyHelper } from "@/helpers/party.helper";
import { PartyService } from "@/services/party.service";

export class FooterPresenter {
  public static getPartyLevel(party: Party): number {
    return PartyHelper.getLevel(party);
  }

  public static getPartyCharisma(party: Party): number {
    return PartyService.getCharisma(party);
  }
}
